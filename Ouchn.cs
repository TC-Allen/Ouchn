using Newtonsoft.Json.Linq;
using Ouchn.Util;
using YamlDotNet.Core.Tokens;
using Object = System.Object;

namespace Ouchn
{
    public partial class Ouchn : Form
    {
        private bool stopCycle = false;
        private NotifyIcon? notifyIcon;

        public Ouchn()
        {
            // 初始化控件
            InitializeComponent();
            // 当速度输入框失去焦点时，更新下一次发送请求的间隔时间
            speedInt.LostFocus += speedInt_LostFocus;
        }

        private void speedInt_LostFocus(object sender, EventArgs e)
        {
            if (int.TryParse(speedInt.Text, out int value)) // 尝试转换为整数
            {
                if (value < 3 || value > 180) // 检查数值是否在范围内
                {
                    MessageBox.Show("速度输入框中的值请在3到180之间的整数");
                    speedInt.Text = "";
                }
            }
        }

        private void Ouchn_Load(object sender, EventArgs e)
        {
            try
            {
                YAML_RW.ReadYAML_Data();

                strCourseid.Text = YAML_RW.yaml_data.courseid;
                strCookie.Text = YAML_RW.yaml_data.cookie;
                speedInt.Text = YAML_RW.yaml_data.speed;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return;
            }

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon("favicon.ico");
            notifyIcon.Text = "Ouchn";
            notifyIcon.Visible = true;

            notifyIcon.ContextMenuStrip = cms;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

        }

        private void ExitItemClick(object sender, EventArgs e)
        {
            YAML_RW.yaml_data.courseid = strCourseid.Text;
            YAML_RW.yaml_data.cookie = strCookie.Text;
            YAML_RW.yaml_data.speed = speedInt.Text;
            YAML_RW.SaveYAML_Data();

            Application.Exit();
        }

        private void Ouchn_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void Ouchn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }

        private void btnStart_Click(Object sender, EventArgs e)
        {
            if (strCourseid.Text == "")
            {
                rtb.AppendText("课程ID为空\r\n");
                return;
            }

            if (strCookie.Text == "")
            {
                rtb.AppendText("Cookie为空\r\n");
                return;
            }
            stopCycle = false;
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            btnStart.Text = "进行中";
            rtb.AppendText("<<<<<<<<<<<<<<<<<<<<========开刷了噢========>>>>>>>>>>>>>>>>>>>>");

            Task.Run(todo);
        }

        private void btnStop_Click(Object sender, EventArgs e)
        {
            stopCycle = true;
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            btnStart.Text = "开始\r\n";
        }

        private void btnClean_Click(Object sender, EventArgs e)
        {
            rtb.Text = "";
        }

        private async void todo()
        {
            try
            {
                ReqClient req = new ReqClient(new HttpClient(), int.Parse(strCourseid.Text), strCookie.Text);
                JObject modulesObj = await req.getMudoles();
                JObject completeActivity = await req.getCompletenes();
                if (modulesObj.TryGetValue("Message", out var msg) ||
                    completeActivity.TryGetValue("Message", out msg))
                {
                    SafeAppendText(msg.ToString());
                    return;
                }

                List<int> completenes =
                    ((JArray)completeActivity["completed_result"]["completed"]["learning_activity"])
                    .Select(item => item.Value<int>())
                    .ToList();
                List<int> moduleIds = modulesObj["modules"].Select(item => item["id"].Value<int>()).ToList();
                JArray activities = await req.getActityByModules(moduleIds);

                for (int i = 0; i < activities.Count; i++)
                {
                    if (stopCycle) break;

                    JObject result = null;
                    JToken token;

                    SafeAppendText("\r\n" + FormatTitle(activities[i]["title"].ToString()));
                    if (completenes.Contains(activities[i]["id"].Value<int>()))
                    {
                        SafeAppendText("done");
                        continue;
                    }

                    if (activities[i]["type"].Value<string>() == "online_video")
                    {
                        VideoData data = new VideoData(0,
                            (int) Math.Ceiling(
                                activities[i]
                                ["uploads"][0]
                                ["videos"][0]
                                ["duration"].Value<decimal>()));
                        result = await req.submitActivity(
                            activities[i]["id"].Value<int>(), data);
                    }

                    if (activities[i]["type"].Value<string>() == "page"
                        || activities[i]["type"].Value<string>() == "web_link")
                    {
                        result = await req.submitActivity(
                            activities[i]["id"].Value<int>(), new Object() {});
                    }

                    if (activities[i]["type"].Value<string>() == "material")
                    {
                        foreach (var file in activities[i]["uploads"])
                        {
                            await req.submitActivity(
                                activities[i]["id"].Value<int>(),
                                new UploadData(file["id"].Value<int>()));
                        }

                        result = await req.submitActivity(
                            activities[i]["id"].Value<int>(),
                            new Object() {});
                    }

                    SafeAppendText("doing...");
                    Thread.Sleep(1000);


                    if (result != null)
                    {
                        if (result.TryGetValue("Message", out token))
                        {
                            SafeAppendText("\r\nAn error occurred while sending the request.!\r\n");
                            --i;
                            Thread.Sleep(2000);
                            continue;
                        }

                        SafeAppendText("done");
                    }
                    else
                    {
                        SafeAppendText("skip");
                        continue;
                    }

                    Thread.Sleep(int.Parse(speedInt.Text == "" ? "3": speedInt.Text) * 1000);
                }

                SafeAppendText($"\r\n<<<<<<<<<<<<<<<<<<<<========{(stopCycle ? "中       止" : "刷完了噢")}========>>>>>>>>>>>>>>>>>>>>\r\n\r\n");
                SafeChangeButton(btnStart, "开始");
                SafeChangeButton(btnStart, true);
                SafeChangeButton(btnStop, false);
            }
            catch
            {
                SafeAppendText("\r\n无效的课程ID\r\n");
                SafeChangeButton(btnStart, "开始");
                SafeChangeButton(btnStart, true);
                SafeChangeButton(btnStop, false);
            }
        }

        private delegate void ButtonChangeCallback(Button btn, object action);

        private void SafeChangeButton(Button btn, object action)
        {
            if (btn.InvokeRequired)
            {
                btn.Invoke(new ButtonChangeCallback(SafeChangeButton), [btn, action]);
            }
            else
            {
                if (action is string)
                {
                    btn.Text = (string)action;
                }
                else
                {
                    btn.Enabled = (bool)action;
                }
            }
        }

        private delegate void AppendTextCallback(string text);

        public void SafeAppendText(string text)
        {
            if (rtb.InvokeRequired)
            {
                rtb.Invoke(new AppendTextCallback(SafeAppendText), [text]);
            }
            else
            {
                rtb.AppendText(text);
            }
        }

        private string FormatTitle(string title)
        {
            string result = title + "\r\n-------------------------------------------->";
            return result;
        }

        private void speedInt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
