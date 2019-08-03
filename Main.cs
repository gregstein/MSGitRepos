using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Newtonsoft.Json;
namespace MSGitRepos
{
    public partial class Main : KryptonForm
    {
        public bool validated = false;
        public string _repostr;
        public string RepoName;
        public string RepoDesc;
        public string RepoStar;
        public string RepoIssue;
        public string RepoUserN;
        public string RepoAvatar;
        public string RepoURL;
        public int CurPage;
        public Main()
        {
            InitializeComponent();
        }

        private async void Main_Load(object sender, EventArgs e)
        {

            Task<int> GrabJson = JsonMain(@"https://api.github.com/search/repositories?q=created:>2017-10-22&sort=stars&order=desc&page=", 1);
            int result = await GrabJson;

            if (validated)
                await Task.Run(() => JsonParser());

        }
      

        #region Json Tasks
        public async void JsonParser()
        {
            try
            {   
                //Clear all
                Invoke(new Action(() => {flowLayoutPanel1.Controls.Clear();}));
                await Task.Delay(100);
                //Setting Control Count
                int n = 30;
                KryptonLinkLabel[] links = new KryptonLinkLabel[n];
                KryptonLabel[] labeldesc = new KryptonLabel[n];
                KryptonLabel[] stargazers = new KryptonLabel[n];
                KryptonLabel[] openissues = new KryptonLabel[n];
                PictureBox[] avatar = new PictureBox[n];
                KryptonLabel[] username = new KryptonLabel[n];
                var jObject = Newtonsoft.Json.Linq.JObject.Parse(_repostr);
                for (int i = 0; i <= 29; i++)
                {

                        RepoName = (string)jObject["items"][i]["full_name"];
                        RepoDesc = (string)jObject["items"][i]["description"];
                        RepoDesc = RepoDesc == null ? string.Empty : RepoDesc.Substring(0, Math.Min(40, RepoDesc.Length)) + " ...";
                        RepoStar = (string)jObject["items"][i]["stargazers_count"];
                        RepoIssue = (string)jObject["items"][i]["open_issues_count"];
                        RepoAvatar = (string)jObject["items"][i]["owner"]["avatar_url"];
                        RepoUserN = (string)jObject["items"][i]["owner"]["login"];
                        RepoURL = (string)jObject["items"][i]["owner"]["html_url"];
                        links[i] = new KryptonLinkLabel();
                        labeldesc[i] = new KryptonLabel();
                        stargazers[i] = new KryptonLabel();
                        openissues[i] = new KryptonLabel();
                        avatar[i] = new PictureBox();
                        username[i] = new KryptonLabel();
                        Invoke(new Action(() =>
                        {
                            //Repo Title
                            links[i].Text = RepoName;
                            links[i].Click += new EventHandler(button_Click);
                            links[i].StateCommon.ShortText.Font = new Font(links[i].Font, FontStyle.Bold);
                            links[i].StateNormal.ShortText.Font = new Font(links[i].Font, FontStyle.Bold);
                            links[i].StateNormal.ShortText.Color1 = Color.Blue;
                            FontFamily fontFamily = new FontFamily("Arial");
                            Font font = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Pixel);
                            links[i].StateNormal.ShortText.Font = font;
                            links[i].StateCommon.ShortText.Font = font;
                            //Repo Description
                            labeldesc[i].Text = RepoDesc;
                            labeldesc[i].StateNormal.ShortText.Color1 = Color.Black;
                            //Icon + Repo Stars
                            PictureBox starico = new PictureBox();
                            starico.Size = new System.Drawing.Size(16, 16);
                            starico.Image = MSGitRepos.Properties.Resources.starico;
                            starico.SizeMode = PictureBoxSizeMode.StretchImage;

                            stargazers[i].Text = RepoStar;
                            //Icon + Repo Issues
                            PictureBox issueico = new PictureBox();
                            issueico.Size = new System.Drawing.Size(16, 16);
                            issueico.Image = MSGitRepos.Properties.Resources.issue;
                            issueico.SizeMode = PictureBoxSizeMode.StretchImage;

                            openissues[i].Text = RepoIssue;
                            //Username & Avatar
                            KryptonLabel bydev = new KryptonLabel();
                            bydev.Text = "Built by";

                            avatar[i].Size = new System.Drawing.Size(16, 16);
                            avatar[i].ImageLocation = RepoAvatar;
                            avatar[i].SizeMode = PictureBoxSizeMode.StretchImage;

                            username[i].Text = RepoUserN;
                            //Merge Controls
                            
                            flowLayoutPanel1.SetFlowBreak(links[i], true);
                            flowLayoutPanel1.Controls.Add(links[i]);
                            

                            flowLayoutPanel1.SetFlowBreak(labeldesc[i], true);
                            flowLayoutPanel1.Controls.Add(labeldesc[i]);

                            flowLayoutPanel1.Controls.Add(starico);
                            flowLayoutPanel1.Controls.Add(stargazers[i]);

                            flowLayoutPanel1.Controls.Add(issueico);
                            flowLayoutPanel1.Controls.Add(openissues[i]);

                            flowLayoutPanel1.Controls.Add(bydev);
                            flowLayoutPanel1.Controls.Add(avatar[i]);

                            flowLayoutPanel1.SetFlowBreak(username[i], true);
                            flowLayoutPanel1.Controls.Add(username[i]);

                            
                            
                        }));
        
                    
                    if (i == 29)
                    {
                        KryptonSeparator splitter = new KryptonSeparator();
                        Invoke(new Action(() => { flowLayoutPanel1.Controls.Add(splitter);}));
                        break;
                    }
                        
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //Nothing
            }
            
            
        }
        async Task<int> JsonMain(string jsonURL, int pageNUM)
        {
            try
            {
                CurPage = pageNUM;
                WebClient Clt = new WebClient();
                Clt.Headers.Add("user-agent", "MSGitRepos");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.Expect100Continue = false; ServicePointManager.MaxServicePointIdleTime = 0;
                _repostr = await Clt.DownloadStringTaskAsync(jsonURL + pageNUM.ToString());
                validated = true;
                return 0;
            }
            catch(Exception ex)
            {
                KryptonMessageBox.Show(ex.ToString());
                validated = false;
                CurPage = 1;
                return 0;
            }
           
        }
        async Task<int> Preloader()
        {
            //Preload Effect
            Invoke(new Action(() => { flowLayoutPanel1.Controls.Clear(); }));
            PictureBox preloader = new PictureBox();
            preloader.Size = new System.Drawing.Size(648, 377);
            preloader.Image = MSGitRepos.Properties.Resources.loading;
            preloader.SizeMode = PictureBoxSizeMode.CenterImage;
            preloader.BackColor = Color.White;
            Invoke(new Action(() => { flowLayoutPanel1.Controls.Add(preloader); }));
            await Task.Delay(500);
            return 0;
        }
        protected void button_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/" + (sender as KryptonLinkLabel).Text);
            
        }
        #endregion

        #region Navigation Events
        private async void Nextbtn_Click(object sender, EventArgs e)
        {
            if(CurPage >= 1)
            {
                //Preload Effect
              
                Task<int> ImgLoad = Preloader();
                int WaitL = await ImgLoad;

                //Enable prev navigation
                Prevbtn.Enabled = true;
                //Parse Json with defined Pagination
                Task<int> GrabJson = JsonMain(@"https://api.github.com/search/repositories?q=created:>2017-10-22&sort=stars&order=desc&page=", CurPage++);
                int result = await GrabJson;

                if (validated)
                    await Task.Run(() => JsonParser());
            }
            CurPage++;
            
        }
        private async void Prevbtn_Click(object sender, EventArgs e)
        {
            if (CurPage == 1)
            {
                Prevbtn.Enabled = false;
            }
            else
            {
                //Preload Effect
                Task<int> ImgLoad = Preloader();
                int WaitL = await ImgLoad;

                
                //Parse Json with defined Pagination
                Task<int> GrabJson = JsonMain(@"https://api.github.com/search/repositories?q=created:>2017-10-22&sort=stars&order=desc&page=", CurPage--);
                int result = await GrabJson;

                if (validated)
                    await Task.Run(() => JsonParser());
            }
            CurPage--;
           
            
        }
        #endregion







        public CancellationToken token { get; set; }
    }
}
