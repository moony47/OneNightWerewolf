using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace OneNightClient {
    public partial class MainForm : Form {
        enum Roles {
            Werewolf, Minion, Mason,
            Seer, Robber, Troublemaker,
            Drunk, Insomniac, Hunter,
            Tanner, Villager, Doppelgänger
        };
        List<string> dupes = new List<string> ();
        Dictionary<string, Image> cards = new Dictionary<string, Image> ();
        Socket readClient;
        Socket writeClient;
        Stream sr;
        Stream sw;
        byte ready = 0;
        bool gameStarted = false;
        string setupState = "";
        int signal;
        int connectedPlayers;
        Roles secretRole;
        List<int> requests = new List<int> ();
        bool inputSent = false;
        Thread readyThread;
        int timerLeft = 0;

        public MainForm () {
            InitializeComponent ();
            dupes.Add ("Werewolf");
            dupes.Add ("Mason");
            dupes.Add ("Villager");
            dupes.Add ("Villager");
            foreach (string x in Enum.GetNames (typeof (Roles))) {
                try {
                    cards.Add (x, Image.FromFile (x + ".png"));
                } catch {
                    MessageBox.Show ("Missing Card Art: " + x + ".png");
                }
            }
        }

        void BtnConnectClick (object sender, EventArgs e) {
            writeClient = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            readClient = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            writeClient.Connect (IPAddress.Parse (txtIP.Text), 3000);
            readClient.Connect (IPAddress.Parse (txtIP.Text), 3001);

            sw = new NetworkStream (writeClient);
            sr = new NetworkStream (readClient);

            if (txtName.Text.Length != 0)
                SendBytes (ConvertString (txtName.Text));
            else
                SendBytes (ConvertString ("Player"));

            btnConnect.Visible = false;
            txtIP.Enabled = false;
            txtName.Visible = false;
            lblIP.Visible = false;
            lblName.Text = ReceiveString();
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            btnReady.Visible = true;
            btnConnect.Text = "Server Found";
            btnReady.Enabled = true;

            readyThread = new Thread (() => {
                do {
                    sw.WriteByte (ready);
                    connectedPlayers = sr.ReadByte ();
                    setupState = connectedPlayers + " players connected\n" +
                        sr.ReadByte () + " players ready";
                    signal = sr.ReadByte ();
                } while (signal == 11);
                gameStarted = true;
                if (signal == 13) {
                    listRoles.Items.Clear ();
                    foreach (string x in Enum.GetNames (typeof (Roles))) {
                        listRoles.Items.Add (x);
                        while (dupes.Contains (x) == true) {
                            listRoles.Items.Add (x);
                            dupes.Remove (x);
                        }
                    }
                }
            });
            readyThread.Start ();
        }

        void DisplayRoleSelection () {
            lblLeft.Visible = true;
            listRoles.Visible = true;
        }

        void BtnReadyClick (object sender, EventArgs e) {
            ready = 1;
            btnReady.Visible = false;
        }

        void TimerTick (object sender, EventArgs e) {
            if (timerLeft > 0) {
                timerLeft--;
                lblSetupState.Text = "It is now the morning. Discuss with the other players and decide who to kill. You have " + ((timerLeft / 60) == 0 ? "" : (timerLeft / 60).ToString () + " minutes ") + (timerLeft % 60) + " seconds before you may cast your vote.";
            } else if (gameStarted == false) {
                lblSetupState.Text = setupState;
                if (connectedPlayers >= 3)
                    btnReady.Enabled = true;
            } else {
                txtIP.Visible = false;
                switch (signal) {
                    case 12:
                        lblSetupState.Text = "All players ready, starting game";
                        DisplaySecretRole ();
                        break;
                    case 13:
                        lblLeft.Visible = true;
                        listRoles.Visible = true;
                        btnStart.Visible = true;
                        lblLeft.Text = (connectedPlayers - listRoles.CheckedItems.Count + 3).ToString () +
                            " roles left to choose";
                        lblSetupState.Text = "All players ready, choose which roles to add";
                        if (listRoles.CheckedItems.Count == connectedPlayers + 3)
                            btnStart.Enabled = true;
                        break;
                    case 14:
                        lblSetupState.Text = "It is currently night time. Your Role is: " + secretRole.ToString ();
                        lblImage.Image = cards[secretRole.ToString ()];
                        lblSetupState.Visible = true;
                        lblImage.Visible = true;

                        lblDisplay0.Visible = false;
                        lblDisplay1.Visible = false;
                        lblDisplay2.Visible = false;
                        lblDisplay3.Visible = false;

                        btnInput0.Visible = false;
                        btnInput1.Visible = false;
                        btnInput2.Visible = false;
                        btnInput3.Visible = false;
                        btnInput4.Visible = false;
                        btnInput5.Visible = false;
                        btnInput6.Visible = false;
                        btnInput7.Visible = false;
                        btnInputM1.Visible = false;
                        btnInputM2.Visible = false;
                        btnInputM3.Visible = false;

                        btnInput0.Enabled = true;
                        btnInput1.Enabled = true;
                        btnInput2.Enabled = true;
                        btnInput3.Enabled = true;
                        btnInput4.Enabled = true;
                        btnInput5.Enabled = true;
                        btnInput6.Enabled = true;
                        btnInput7.Enabled = true;
                        btnInputM1.Enabled = true;
                        btnInputM2.Enabled = true;
                        btnInputM3.Enabled = true;
                        signal = 0;
                        break;
                    case 15:
                        lblImage.Visible = false;
                        for (int x = 0; x < requests.Count; x++) {
                            switch (requests[x]) {
                                case 0:
                                    //Show mid buttons
                                    btnInputM1.Visible = true;
                                    btnInputM2.Visible = true;
                                    btnInputM3.Visible = true;
                                    do {
                                        x++;
                                        if (requests[x] != 127)
                                            ((Button) Controls.Find ("BtnInputM" + requests[x], true)[0]).Enabled = false;
                                    } while (requests[x] != 127);

                                    inputSent = false;
                                    break;
                                case 1:
                                    //Show player buttons
                                    //Name for each button in next elements
                                    for (int y = 0; y < connectedPlayers; y++) {
                                        ((Button) Controls.Find ("BtnInput" + y, true)[0]).Text = "";
                                        do {
                                            x++;
                                            if (requests[x] != 126 && requests[x] != 127 && requests[x] != 128)
                                                ((Button) Controls.Find ("BtnInput" + y, true)[0]).Text += (char) requests[x];
                                        } while (requests[x] != 126 && requests[x] != 127 && requests[x] != 128);
                                        if (((Button) Controls.Find ("BtnInput" + y, true)[0]).Text.StartsWith ("%SKIP%")) {
                                            ((Button) Controls.Find ("BtnInput" + y, true)[0]).Text = ((Button) Controls.Find ("BtnInput" + y, true)[0]).Text.Substring (6);
                                            ((Button) Controls.Find ("BtnInput" + y, true)[0]).Enabled = false;
                                        }
                                        ((Button) Controls.Find ("BtnInput" + y, true)[0]).Visible = true;
                                    }
                                    inputSent = false;
                                    break;
                                case 2:
                                    //Instructions
                                    string ins = "";
                                    do {
                                        x++;
                                        if (requests[x] != 126)
                                            ins += (char) requests[x];
                                    } while (requests[x] != 126);
                                    lblSetupState.Text = ins;
                                    lblSetupState.Visible = true;
                                    break;
                                case 3:
                                    lblSetupState.Visible = false;
                                    //Next element is role to display
                                    List<Roles> r = new List<Roles> ();
                                    do {
                                        x++;
                                        if (requests[x] != 126)
                                            r.Add ((Roles) requests[x]);
                                    } while (requests[x] != 126);

                                    //Name of player the roles are of in next elements
                                    List<string> n = new List<string> ();
                                    do {
                                        n.Add ("");
                                        do {
                                            x++;
                                            if (requests[x] != 126 && requests[x] != 127 && requests[x] != 128)
                                                n[n.Count - 1] += (char) requests[x];
                                        } while (requests[x] != 126 && requests[x] != 127 && requests[x] != 128);
                                    } while (requests[x] != 127 && requests[x] != 128);

                                    for (int y = 0; y < r.Count; y++) {
                                        ((Label) Controls.Find ("lblDisplay" + y, true)[0]).Text = n[y] + r[y].ToString ();
                                        ((Label) Controls.Find ("lblDisplay" + y, true)[0]).Image = cards[r[y].ToString ()];
                                        ((Label) Controls.Find ("lblDisplay" + y, true)[0]).Visible = true;
                                    }
                                    inputSent = true;
                                    break;
                                case 4:
                                    //End night
                                    timer.Interval = 1000;
                                    timerLeft = 24 * connectedPlayers - 12;

                                    lblSetupState.Text = "It is now the morning. Discuss with the other players and decide who to kill. You have " + ((timerLeft / 60) == 0 ? "": (timerLeft / 60).ToString() + " minutes") + (timerLeft % 60) + " seconds before you may cast your vote.";
                                    lblSetupState.Visible = true;

                                    btnInput0.Enabled = false;
                                    btnInput1.Enabled = false;
                                    btnInput2.Enabled = false;
                                    btnInput3.Enabled = false;
                                    btnInput4.Enabled = false;
                                    btnInput5.Enabled = false;
                                    btnInput6.Enabled = false;
                                    btnInput7.Enabled = false;

                                    new Thread (() => {
                                        signal = 0;
                                        Thread.Sleep (24000 * connectedPlayers - 12000);
                                        requests.Clear ();
                                        requests.AddRange (ReceiveBytes ());
                                        signal = 15;
                                    }).Start ();
                                    break;
                                case 5:
                                    //Lynch time
                                    lblSetupState.Text = "Now vote on who you think should be killed";
                                    lblSetupState.Visible = true;

                                    for (int y = 0; y < connectedPlayers; y++)
                                        if (((Button) Controls.Find ("BtnInput" + y, true)[0]).Text != lblName.Text)
                                            ((Button) Controls.Find ("BtnInput" + y, true)[0]).Enabled = true;

                                    inputSent = false;
                                    break;
                                case 6:
                                    //Restart App after delay
                                    signal = 0;
                                    new Thread (() => {
                                        Thread.Sleep (5000);
                                        Application.Restart ();
                                    }).Start ();
                                    break;
                                case 128:
                                    new Thread (() => {
                                        signal = 0;
                                        do {
                                            Thread.Sleep (6000);
                                        } while (inputSent == false);
                                        WaitForNightAction ();
                                    }).Start ();
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        byte[] ConvertString (string x) {
            byte[] s = new byte[x.Length];
            for (int y = 0; y < x.Length; y++) {
                s[y] = (byte) x[y];
            }
            return s;
        }

        void SendBytes (byte[] msg) {
            //Sum up the bytes to use as checksum
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
                sw.Write (toSend.ToArray (), 0, toSend.Count);
                while (response != 126 && response != 127){
                    response = sr.ReadByte ();
                }
            }
        }

        string ReceiveString () {
            string s = "";
            int[] bytes = ReceiveBytes ();

            if (bytes == null)
                return "Player";

            foreach (byte x in bytes)
                s += (char) x;
            return s;
        }

        int[] ReceiveBytes () {
            try {
                int total = 0;
                int checksum = 0;
                List<int> s = new List<int> ();
                do {
                    int temp = 0;
                    do {
                        temp = sr.ReadByte ();
                    } while (temp != 129);
                    int length = sr.ReadByte ();
                    byte[] msg = new byte[length];
                    sr.Read (msg, 0, length);
                    checksum = sr.ReadByte ();

                    total = 0;
                    s.Clear ();
                    foreach (byte x in msg) {
                        total += x;
                        s.Add (x);
                    }

                    if (checksum != (total % 256))
                        sw.WriteByte (126);
                } while (checksum != (total % 256));
                sw.WriteByte (127);
                return s.ToArray ();
            } catch {
                sw.WriteByte (127);
                return null;
            }
        }

        void InputClick (object sender, EventArgs e) {
            btnInput0.Enabled = false;
            btnInput1.Enabled = false;
            btnInput2.Enabled = false;
            btnInput3.Enabled = false;
            btnInput4.Enabled = false;
            btnInput5.Enabled = false;
            btnInput6.Enabled = false;
            btnInput7.Enabled = false;
            btnInputM1.Enabled = false;
            btnInputM2.Enabled = false;
            btnInputM3.Enabled = false;

            Button btn = (Button) sender;
            SendBytes (ConvertString (btn.Name.Substring (8)));
            inputSent = true;
        }

        void DisplaySecretRole () {
            new Thread (() => {
                readyThread.Abort ();
                signal = 0;
                secretRole = (Roles) sr.ReadByte ();
                WaitForNightAction ();
            }).Start ();
        }

        void WaitForNightAction () {
            signal = 14;
            Thread.Sleep (500);
            requests.Clear ();
            requests.AddRange (ReceiveBytes ());
            signal = 15;
        }

        void ListRolesItemCheck (object sender, ItemCheckEventArgs e) {
            if (listRoles.CheckedItems.Count >= connectedPlayers + 3 && e.NewValue == CheckState.Checked)
                e.NewValue = CheckState.Unchecked;
        }

        void BtnStartClick (object sender, EventArgs e) {
            sw.WriteByte (126);
            foreach (string x in listRoles.CheckedItems) {
                sw.WriteByte ((byte) (int) Enum.Parse (typeof (Roles), x));
            }
            sw.WriteByte (127);
            listRoles.Visible = false;
            lblLeft.Visible = false;
            btnStart.Visible = false;
            DisplaySecretRole ();
        }
    }
}