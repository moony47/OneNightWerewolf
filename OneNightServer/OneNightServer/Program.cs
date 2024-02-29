using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Threading;

namespace OneNightServer {
    class Program {
        enum Roles {
            Werewolf, Minion, Mason,
            Seer, Robber, Troublemaker,
            Drunk, Insomniac, Hunter,
            Tanner, Villager, Doppelgänger
        };
        const int maxPlayers = 8;
        const int minPlayers = 3;
        static Socket readServer;
        static Socket writeServer;
        static bool stop = false;
        static List<Stream> sr = new List<Stream> ();
        static List<Stream> sw = new List<Stream> ();
        static List<string> names = new List<string> ();
        static int readyPlayers = 0;
        static bool stateChanged = false;
        static List<Roles> gameRoles = new List<Roles> ();
        static List<Roles> startRoles = new List<Roles> ();
        static List<Roles> endRoles = new List<Roles> ();
        static Random rn = new Random ();
        static bool readerLock = false;
        static Thread connect1;
        static Thread connect2;

        public static void Main (string[] args) {
            string ip = new WebClient ().DownloadString ("https://api.ipify.org");
            Console.WriteLine ("Your public IP is: " + ip);
            Console.WriteLine ("Open ports 3000-3001 and press enter to start server: ");
            Console.ReadKey ();

            SetupServer (ip);
            CheckForStart ();
            StartGame ();

            Console.ReadKey (true);
        }

        static void StartGame () {
            new Thread (() => {
                sr[0].ReadTimeout = 100000;
                int r;
                do {
                    r = sr[0].ReadByte ();
                } while (r != 126);
                do {
                    r = sr[0].ReadByte ();
                    if (r != 127)
                        gameRoles.Add ((Roles) r);
                } while (r != 127);

                do {
                    int index = rn.Next (0, gameRoles.Count);
                    startRoles.Add (gameRoles[index]);
                    gameRoles.RemoveAt (index);
                } while (gameRoles.Count > 0);

                for (int x = 0; x < sw.Count; x++) {
                    sw[x].WriteByte ((byte) startRoles[x]);
                    Console.WriteLine (startRoles[x].ToString ());
                }

                Console.WriteLine ("\nMiddle Cards:");
                Console.WriteLine (startRoles[startRoles.Count - 1].ToString ());
                Console.WriteLine (startRoles[startRoles.Count - 2].ToString ());
                Console.WriteLine (startRoles[startRoles.Count - 3].ToString ());

                endRoles.AddRange (startRoles.ToArray ());
                int doppelInsomniac = -1;

                //Doppelgänger Phase (Copying)
                Thread.Sleep (5000);

                for (int x = 0; x < sw.Count; x++) {
                    if (startRoles[x] == Roles.Doppelgänger) {
                        DoppelgangerPhase (x);
                        if (endRoles[x] == Roles.Insomniac)
                            doppelInsomniac = x;
                        break;
                    }
                }

                //Werewolf-Mason-Minion-Seer Phase (Viewing)
                Thread.Sleep (3000);

                for (int x = 0; x < sw.Count; x++) {
                    if (endRoles[x] == Roles.Werewolf) {
                        WerewolfPhase (x);
                    }
                }

                for (int x = 0; x < sw.Count; x++) {
                    if (startRoles[x] == Roles.Minion) {
                        MinionPhase (x);
                        break;
                    }
                }

                for (int x = 0; x < sw.Count; x++) {
                    if (endRoles[x] == Roles.Mason) {
                        MasonPhase (x);
                    }
                }

                for (int x = 0; x < sw.Count; x++) {
                    if (startRoles[x] == Roles.Seer) {
                        SeerPhase (x);
                        break;
                    }
                }

                //Robber-Troublemaker-Drunk Phase (Swapping)
                Thread.Sleep (3000);

                for (int x = 0; x < sw.Count; x++) {
                    if (startRoles[x] == Roles.Robber) {
                        RobberPhase (x);
                        break;
                    }
                }

                for (int x = 0; x < sw.Count; x++) {
                    if (startRoles[x] == Roles.Troublemaker) {
                        TroublemakerPhase (x);
                        break;
                    }
                }

                for (int x = 0; x < sw.Count; x++) {
                    if (startRoles[x] == Roles.Drunk) {
                        DrunkPhase (x);
                        break;
                    }
                }

                //Insomniac Phase (Wake up view)
                Thread.Sleep (3000);

                for (int x = 0; x < sw.Count; x++) {
                    if (startRoles[x] == Roles.Insomniac) {
                        InsomniacPhase (x);
                        break;
                    }
                }

                if (doppelInsomniac != -1)
                    InsomniacPhase (doppelInsomniac);

                Thread.Sleep (3000);

                //End the night for all players
                for (int x = 0; x < sw.Count; x++) {
                    List<byte> msg = new List<byte> ();
                    msg.Add (1);
                    foreach (string n in names) {
                        msg.AddRange (ConvertString (n));
                        msg.Add (126);
                    }
                    msg.Add (4);
                    SendBytes (x, msg.ToArray ());
                }

                //Allow discussion for 1 minute
                Thread.Sleep (24000 * sw.Count - 12000);

                //Ask for player kill votes
                for (int x = 0; x < sw.Count; x++) {
                    List<byte> msg = new List<byte> ();
                    msg.Add (1);
                    foreach (string n in names) {
                        msg.AddRange (ConvertString (n));
                        msg.Add (126);
                    }
                    msg.Add (5);
                    msg.Add (128);
                    SendBytes (x, msg.ToArray ());
                }

                //Count Votes
                int hunterVote = -1;
                int[] votes = new int[sw.Count];
                for (int x = 0; x < sw.Count; x++) {
                    int selection = int.Parse (ReceiveString (x, 10000000));
                    votes[selection]++;
                    if (endRoles[x] == Roles.Hunter)
                        hunterVote = x;
                }

                //Find highest number of voted players
                List<int> highestIndices = new List<int> ();
                int highestVotes = 0;
                for (int x = 0; x < sw.Count; x++) {
                    if (votes[x] == highestVotes) {
                        highestIndices.Add (x);
                        highestVotes = votes[x];
                    } else if (votes[x] > highestVotes) {
                        highestIndices.Clear ();
                        highestIndices.Add (x);
                        highestVotes = votes[x];
                    }
                }

                if (highestVotes > 1) {
                    foreach (int x in highestIndices)
                        if (hunterVote != -1 && endRoles[x] == Roles.Hunter)
                            if (highestIndices.Contains (hunterVote) == false)
                                highestIndices.Add (hunterVote);
                            else
                                hunterVote = -1;

                    for (int x = 0; x < sw.Count; x++) {
                        List<byte> msg = new List<byte> ();
                        msg.Add (3);
                        foreach (int y in highestIndices) {
                            msg.Add ((byte) endRoles[y]);
                        }
                        msg.Add (126);
                        foreach (int y in highestIndices) {
                            msg.AddRange (ConvertString (names[y] + " was killed " + (y == hunterVote ? "by the Hunter" : ("with " + highestVotes + " votes")) + " and was the "));
                            msg.Add (126);
                        }
                        msg.Add (127);
                        msg.Add (6);
                        SendBytes (x, msg.ToArray ());
                    }
                } else {
                    for (int x = 0; x < sw.Count; x++) {
                        List<byte> msg = new List<byte> ();
                        msg.Add (2);
                        msg.AddRange (ConvertString ("Everyone was voted for once and so no one was killed."));
                        msg.Add (126);
                        msg.Add (6);
                        SendBytes (x, msg.ToArray ());
                    }
                }

            }).Start ();

            Console.ReadKey (true);
        }

        static void SwapRoles (int x, int y) {
            Roles temp = endRoles[x];
            endRoles[x] = endRoles[y];
            endRoles[y] = temp;
        }

        static void DoppelgangerPhase (int x) {
            List<byte> msg = new List<byte> ();
            msg.Add (2);
            msg.AddRange (ConvertString ("You are the Doppelgänger. Please choose a player to copy the role of:"));
            msg.Add (126);
            msg.Add (1);
            foreach (string n in names) {
                if (names[x] == n)
                    msg.AddRange (ConvertString ("%SKIP%"));
                msg.AddRange (ConvertString (n));
                msg.Add (126);
            }
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
            msg.Clear ();

            int selection = int.Parse(ReceiveString (x, 10000000));
            endRoles[x] = endRoles[selection];

            msg.Add (3);
            msg.Add ((byte) endRoles[x]);
            msg.Add (126);
            msg.AddRange (ConvertString ("You copied the role of " + names[selection] + " and are now the "));
            msg.Add (126);
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());

            switch (endRoles[x]) {
                case Roles.Seer:
                    SeerPhase (x);
                    break;
                case Roles.Robber:
                    RobberPhase (x);
                    break;
                case Roles.Troublemaker:
                    TroublemakerPhase (x);
                    break;
                case Roles.Drunk:
                    DrunkPhase (x);
                    break;
            }
        }

        static void WerewolfPhase (int x) {
            List<byte> msg = new List<byte> ();
            int count = 0;
            msg.Add (3);
            for (int y = 0; y < sw.Count; y++) {
                if (endRoles[y] == Roles.Werewolf) {
                    msg.Add ((byte) Roles.Werewolf);
                    count++;
                }
            }
            msg.Add (126);
            for (int y = 0; y < sw.Count; y++) {
                if (endRoles[y] == Roles.Werewolf) {
                    if (x != y)
                        msg.AddRange (ConvertString(names[y] + " is a "));
                    else
                        msg.AddRange (ConvertString (count == 1 ? "You are the only " : "You are a "));
                    msg.Add (126);
                }
            }
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray());
            msg.Clear ();

            if (count == 1) {
                msg.Add (2);
                msg.AddRange (ConvertString ("You are the only Werewolf. Please choose a middle card to view:"));
                msg.Add (126);
                msg.Add (0);
                msg.Add (127);
                msg.Add (128);
                SendBytes (x, msg.ToArray ());
                msg.Clear ();

                string selection = ReceiveString (x, 10000000);

                msg.Add (3);
                msg.Add ((byte) endRoles[endRoles.Count - int.Parse (selection.Substring (1))]);
                msg.Add (126);
                msg.AddRange (ConvertString ("Middle " + selection.Substring(1) + " is a "));
                msg.Add (126);
                msg.Add (127);
                msg.Add (128);
                SendBytes (x, msg.ToArray ());
            }
        }

        static void MinionPhase (int x) {
            List<byte> msg = new List<byte> ();
            int count = 0;
            msg.Add (3);
            for (int y = 0; y < sw.Count; y++) {
                if (endRoles[y] == Roles.Werewolf) {
                    msg.Add ((byte) Roles.Werewolf);
                    count++;
                }
            }
            if (count == 0)
                msg.Add (255);
            msg.Add (126);
            if (count > 0) {
                for (int y = 0; y < sw.Count; y++) {
                    if (endRoles[y] == Roles.Werewolf) {
                        msg.AddRange (ConvertString (names[y] + " is a "));
                        msg.Add (126);
                    }
                }
            } else {
                msg.AddRange (ConvertString ("There are no werewolves"));
            }
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
        }

        static void MasonPhase (int x) {
            List<byte> msg = new List<byte> ();
            int count = 0;
            msg.Add (3);
            for (int y = 0; y < sw.Count; y++) {
                if (endRoles[y] == Roles.Mason) {
                    msg.Add ((byte) Roles.Mason);
                    count++;
                }
            }
            msg.Add (126);
            for (int y = 0; y < sw.Count; y++) {
                if (endRoles[y] == Roles.Mason) {
                    if (x != y)
                        msg.AddRange (ConvertString (names[y] + " is a "));
                    else
                        msg.AddRange (ConvertString (count == 1 ? "You are the only " : "You are a "));
                    msg.Add (126);
                }
            }
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
        }

        static void SeerPhase (int x) {
            List<byte> msg = new List<byte> ();
            msg.Add (2);
            msg.AddRange (ConvertString ("You are the Seer. Please choose a player card to view or two middle card to view:"));
            msg.Add (126);
            msg.Add (0);
            msg.Add (127);
            msg.Add (1);
            foreach (string n in names) {
                if (names[x] == n)
                    msg.AddRange (ConvertString ("%SKIP%"));
                msg.AddRange (ConvertString (n));
                msg.Add (126);
            }
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
            msg.Clear ();

            string selection = ReceiveString (x, 10000000);

            if (selection[0] == 'M') {
                //If chose a mid card ask for another mid card
                msg.Add (2);
                msg.AddRange (ConvertString ("You are the Seer. Please choose a second middle card to view:"));
                msg.Add (126);
                msg.Add (0);
                msg.Add (byte.Parse(selection.Substring(1)));
                msg.Add (127);
                msg.Add (128);
                SendBytes (x, msg.ToArray ());
                msg.Clear ();

                string selection2 = ReceiveString (x, 10000000);

                msg.Add (3);
                msg.Add ((byte) endRoles[endRoles.Count- int.Parse (selection.Substring (1))]);
                msg.Add ((byte) endRoles[endRoles.Count - int.Parse (selection2.Substring (1))]);
                msg.Add (126);
                msg.AddRange (ConvertString ("Middle  " + selection.Substring (1) + " is a "));
                msg.Add (126);
                msg.AddRange (ConvertString ("Middle  " + selection2.Substring (1) + " is a "));
                msg.Add (126);
                msg.Add (127);
                msg.Add (128);
                SendBytes (x, msg.ToArray ());
            } else {
                //If chose a player card
                msg.Add (3);
                msg.Add ((byte) endRoles[int.Parse(selection)]);
                msg.Add (126);
                msg.AddRange (ConvertString (names[int.Parse (selection)] + " is a "));
                msg.Add (126);
                msg.Add (127);
                msg.Add (128);
                SendBytes (x, msg.ToArray());
            }
        }

        static void RobberPhase (int x) {
            List<byte> msg = new List<byte> ();
            msg.Add (2);
            msg.AddRange (ConvertString ("You are the Robber. Please choose a player to swap roles with:"));
            msg.Add (126);
            msg.Add (1);
            foreach (string n in names) {
                if (names[x] == n)
                    msg.AddRange (ConvertString ("%SKIP%"));
                msg.AddRange (ConvertString (n));
                msg.Add (126);
            }
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
            msg.Clear ();

            int selection = int.Parse (ReceiveString (x, 10000000));

            msg.Add (3);
            msg.Add ((byte) endRoles[selection]);
            msg.Add (126);
            msg.AddRange(ConvertString ("You swapped with " + names[selection] + " who was a "));
            msg.Add (126);
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());

            SwapRoles (x, selection);
        }

        static void TroublemakerPhase (int x) {
            List<byte> msg = new List<byte> ();
            msg.Add (2);
            msg.AddRange (ConvertString ("You are the Troublemaker. Please choose two players to switch the roles of:"));
            msg.Add (126);
            msg.Add (1);
            foreach (string n in names) {
                if (names[x] == n)
                    msg.AddRange (ConvertString ("%SKIP%"));
                msg.AddRange (ConvertString (n));
                msg.Add (126);
            }
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
            msg.Clear ();

            int selection1 = int.Parse (ReceiveString (x, 10000000));

            msg.Add (2);
            msg.AddRange (ConvertString ("You are the Troublemaker. Please choose a second player:"));
            msg.Add (126);
            msg.Add (1);
            foreach (string n in names) {
                if (names[x] == n || names[selection1] == n)
                    msg.AddRange (ConvertString ("%SKIP%"));
                msg.AddRange (ConvertString (n));
                msg.Add (126);
            }
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
            msg.Clear ();

            int selection2 = int.Parse (ReceiveString (x, 10000000));

            SwapRoles (selection1, selection2);

            msg.Add (2);
            msg.AddRange (ConvertString("You swapped " + names[selection1] + " and " + names[selection2]));
            msg.Add (126);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
        }

        static void DrunkPhase (int x) {
            List<byte> msg = new List<byte> ();
            msg.Add (2);
            msg.AddRange (ConvertString ("You are the Drunk. Please choose a middle card to swap your role with:"));
            msg.Add (126);
            msg.Add (0);
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
            msg.Clear ();

            string selection = ReceiveString (x, 10000000);

            SwapRoles (x, endRoles.Count - int.Parse (selection.Substring (1)));

            msg.Add (2);
            msg.AddRange (ConvertString("You swapped role with Middle " + (endRoles.Count - int.Parse (selection.Substring (1))).ToString ()));
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
        }

        static void InsomniacPhase (int x) {
            List<byte> msg = new List<byte> ();
            msg.Add (3);
            msg.Add ((byte) endRoles[x]);
            msg.Add (126);
            msg.AddRange(ConvertString("You wake up to find you are the "));
            msg.Add (126);
            msg.Add (127);
            msg.Add (128);
            SendBytes (x, msg.ToArray ());
        }

        static byte[] ConvertString (string x) {
            byte[] s = new byte[x.Length];
            for (int y = 0; y < x.Length; y++) {
                s[y] = (byte) x[y];
            }
            return s;
        }

        static void SendBytes (int index, byte[] msg) {
            //Sum up the bytes to use ass checksum
            int total = 0;
            foreach (byte x in msg)
                total += x;

            //Add bytes to one list and send array in one go
            List<byte> toSend = new List<byte> ();
            toSend.Add (129);
            toSend.Add ((byte) msg.Length);
            toSend.AddRange (msg);
            toSend.Add ((byte) (total % 256));

            int response = 0;
            //Resend if not received in full
            while (response != 127) {
                sw[index].Write (toSend.ToArray (), 0, toSend.Count);
                while (response != 126 && response != 127) {
                    response = sr[index].ReadByte ();
                }
            }
        }

        static string ReceiveString (int index, int timeout = 10000) {
            string s = "";
            int[] bytes = ReceiveBytes (index, timeout);

            if (bytes == null)
                return "Player";

            foreach (byte x in bytes)
                s += (char) x;

            return s;
        }

        static int[] ReceiveBytes (int index, int timeout = 10000) {
            sr[index].ReadTimeout = timeout;
            try {
                int total = 0;
                int checksum = 0;
                List<int> s = new List<int> ();
                do {
                    int temp = 0;
                    do {
                        temp = sr[index].ReadByte ();
                    } while (temp != 129);
                    int length = sr[index].ReadByte ();
                    byte[] msg = new byte[length];
                    sr[index].Read (msg, 0, length);
                    checksum = sr[index].ReadByte ();

                    total = 0;
                    s.Clear ();
                    foreach (byte x in msg) {
                        total += x;
                        s.Add (x);
                    }

                    if (checksum != (total % 256))
                        sw[index].WriteByte (126);
                } while (checksum != (total % 256));
                sw[index].WriteByte (127);
                return s.ToArray ();
            } catch {
                sw[index].WriteByte (127);
                return null;
            }
        }

        static void SetupServer (string ip) {
            stateChanged = true;

            new Thread (() => {
                while (stop == false) {
                    if (stateChanged == false || stop == true)
                        continue;

                    Console.Clear ();
                    stateChanged = false;
                    Console.WriteLine (
                        ip + "\nWaiting for connections...\n" +
                        (sr.Count > 0 ? sr.Count + " players connected\n" : "") +
                        (readyPlayers > 0 ? readyPlayers + " players ready\n" : "\n")
                    );
                    foreach (string n in names)
                        Console.WriteLine (n);
                }
                Console.Clear ();
                Console.WriteLine ("Game Started with " + sr.Count + " players:\n");
            }).Start ();

            connect1 = new Thread (() => {
                readServer = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                readServer.Bind (new IPEndPoint (IPAddress.Any, 3000));
                readServer.Listen (maxPlayers);
                do {
                    Socket newClient = readServer.Accept ();
                    Stream temp = new NetworkStream (newClient);
                    sr.Add (temp);
                    do {
                        Thread.Sleep (1);
                    } while (sr.Count > sw.Count);
                    readerLock = true;
                    string s = ReceiveString (sr.IndexOf (temp));
                    names.Add (s == "Player" ? s + (1 + sr.IndexOf (temp)) : s);
                    SendBytes (names.Count - 1, ConvertString (names[names.Count-1]));
                    readerLock = false;
                } while (stop == false || sr.Count == maxPlayers);
            });
            connect1.Start ();

            connect2 = new Thread (() => {
                writeServer = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                writeServer.Bind (new IPEndPoint (IPAddress.Any, 3001));
                writeServer.Listen (maxPlayers);
                do {
                    Socket newClient = writeServer.Accept ();
                    Stream temp = new NetworkStream (newClient);
                    sw.Add (temp);
                    do {
                        Thread.Sleep (1);
                    } while (sw.Count > sr.Count);
                    stateChanged = true;
                } while (stop == false || sw.Count == maxPlayers);
            });
            connect2.Start ();
        }

        static void CheckForStart () {
            bool ready = true;
            int count = 0;
            do {
                ready = true;
                count = 0;
                for (int x = 0; x < sw.Count; x++) {
                    do {
                        Thread.Sleep (1);
                    } while (readerLock == true);
                    if (sr[x].ReadByte () == 0)
                        ready = false;
                    else
                        count++;

                    sw[x].WriteByte ((byte) sr.Count);
                    sw[x].WriteByte ((byte) readyPlayers);
                    sw[x].WriteByte (11);
                }

                if (readyPlayers != count) {
                    stateChanged = true;
                    readyPlayers = count;
                }
            } while (ready == false || sr.Count < minPlayers);

            sw[0].WriteByte ((byte) sr.Count);
            sw[0].WriteByte ((byte) readyPlayers);
            sw[0].WriteByte (13);

            for (int x = 1; x < sw.Count; x++) {
                sw[x].WriteByte ((byte) sr.Count);
                sw[x].WriteByte ((byte) readyPlayers);
                sw[x].WriteByte (12);
            }

            stop = true;
        }
    }
}