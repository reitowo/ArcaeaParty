using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArcaeaParty
{
    public partial class ArcaeaParty : Form
    {
        const string AppVersion = "2.3.0";
        const string DeviceId = "<---->";

        private HttpClient client = new HttpClient();
        private string AccessToken = "<---->";
        private string ArcApiEndpoint = "https://arcapi.lowiro.com/7/";
        private string currentMapId = null;
        private Dictionary<string, WorldMap> worldMaps = new Dictionary<string, WorldMap>();
        private int remainStamina;

        delegate void JObjectReceiver(JObject j);
        delegate void StringReceiver(string s);

        public ArcaeaParty()
        {
            InitializeComponent();
            client.BaseAddress = new Uri(ArcApiEndpoint);
            Initialize();
        }

        public async void Initialize()
        {
            //PlayNormal();
            await Login("iAmNotCheater", "iAmNotCheater"); 
            //await SelfDestruct(); 
            string songToken = await GetSongToken(); //获取一个提交的令牌
            await SubmitScore(songToken, GetSongHash("onelastdrive"), "onelastdrive", 885); //提交成绩

            JObject userInfo = await GetUserInfo();
            Invoke(new JObjectReceiver(UpdateUserInfo), userInfo);
            JObject allMapList = await GetAllMapList();
            WorldMapTree.Invoke(new JObjectReceiver(UpdateWorldMapTree), allMapList);
            new Thread(AutoPlay).Start();
        }

        public async Task SelfDestruct()
        {
            string songToken = await GetSongToken();
            await SubmitScore(songToken, "dcaffc07096884df87cb8533c3746ba5", "grievouslady", 1450);
            songToken = await GetSongToken();
            await SubmitScore(songToken, "dcaffc07096884df87cb8533c3746ba5", "grievouslady", 1450);
            songToken = await GetSongToken();
            await SubmitScore(songToken, "dcaffc07096884df87cb8533c3746ba5", "grievouslady", 1450);
        }
        public async Task Login(string name, string password)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{name}:{password}")));
            client.DefaultRequestHeaders.TryAddWithoutValidation("DeviceId", DeviceId);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion); Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            keyValuePairs.Add("grant_type", "client_credentials");
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(keyValuePairs);

            HttpResponseMessage response = await client.PostAsync("auth/login", formUrlEncodedContent);
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            if (json.ContainsKey("success") && (((bool)json["success"]) == true))
            {
                AccessToken = (string)json["access_token"];
            }
        }
        public async Task<JObject> GetUserInfo()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            HttpResponseMessage response = await client.GetAsync("user/me");
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return json;
        }
        public async Task<JObject> SetCurrentCharactor(int id)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { "character", id.ToString() },
                { "skill_sealed", "false" }
            };

            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(keyValuePairs);
            HttpResponseMessage response = await client.PostAsync("world/map/me", formUrlEncodedContent);
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return json;
        }
        public async Task<JObject> SetCurrentMap(string mapid)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { "map_id", mapid }
            };

            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(keyValuePairs);
            HttpResponseMessage response = await client.PostAsync("world/map/me", formUrlEncodedContent);
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return json;
        }
        public async Task<JObject> GetAllMapList()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            HttpResponseMessage response = await client.GetAsync("world/map/me");
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return json;
        }
        public async Task<JObject> GetSpecificMap(string mapid)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            HttpResponseMessage response = await client.GetAsync($"world/map/me/{mapid}");
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return json;
        }
        public async Task<JObject> GetWorldRanking(string songid)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            HttpResponseMessage response = await client.GetAsync($"score/song?song_id={songid}&difficulty=2&start=0&limit=10");
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return json;
        }
        public async Task<string> GetSongToken()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            HttpResponseMessage response = await client.GetAsync("score/token");
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            if (json.ContainsKey("success") && (((bool)json["success"]) == true))
            {
                return (string)json["value"]["token"];
            }

            return null;
        }
        public async Task<JObject> GetWorldSongToken(string songid)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            HttpResponseMessage response = await client.GetAsync($"score/token/world?songid={songid}&difficulty=2");
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return json;
        }
        public async Task<JObject> ConvertFragmentStamina()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            StringContent st = new StringContent("");

            HttpResponseMessage response = await client.PostAsync("purchase/me/stamina/fragment", st);
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return json;
        }
        public async Task<JObject> SubmitScore(string songToken, string songHash, string songid, int noteCount, int shiny = 0)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("AppVersion", AppVersion);

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { "song_token", songToken },
                {"song_hash", songHash},
                {"song_id" ,songid},
                {"difficulty", "2" },
                {"score", (10000000 + shiny).ToString("D8")},
                {"shiny_perfect_count",shiny.ToString() },
                {"perfect_count",noteCount.ToString() },
                {"near_count", "0"},
                {"miss_count","0" },
                {"health","100" },
                {"modifier","0" },
            };

            string toHash = $"{songToken}{songHash}{songid}2{keyValuePairs["score"]}{keyValuePairs["shiny_perfect_count"]}{keyValuePairs["perfect_count"]}001000moeuguu";
            string submissionHash = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(toHash))).Replace("-", "").ToLower();
            keyValuePairs.Add("submission_hash", submissionHash);

            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(keyValuePairs);

            HttpResponseMessage response = await client.PostAsync("score/song", formUrlEncodedContent);
            JObject json = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return json;
        }

        public string GetSongHash(string songid)
        {
            string text = File.ReadAllText(@"H:\Arcaea\arcaea_1.9.0c\assets\songs\" + songid + @"\2.aff");
            return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "").ToLower();
        }
        public async void SetCurrentMapId(string mapId)
        {
            currentMapId = mapId;
            CurrentMap.Text = mapId;
            JObject mapInfo = await GetSpecificMap(mapId);
            WorldMapTree.Invoke(new JObjectReceiver(UpdateCurrentMap), mapInfo);
        }
        public async void PlayParty()
        {
            string songid = "flyburg";
            int noteCount = (await GetWorldRanking(songid))["value"][0]["perfect_count"].Value<int>();
            string hash = GetSongHash(songid);
            Step s = worldMaps[currentMapId].steps[worldMaps[currentMapId].curr_position];
            if (s.restrict_type != null)
            {
                switch (s.restrict_type)
                {
                    case "song_id":
                        songid = s.restrict_id;
                        hash = GetSongHash(songid);
                        noteCount = (await GetWorldRanking(songid))["value"][0]["perfect_count"].Value<int>();
                        break;
                    case "pack_id":
                        songid = "merlin";
                        hash = GetSongHash(songid);
                        noteCount = (await GetWorldRanking(songid))["value"][0]["perfect_count"].Value<int>();
                        break;
                    default:
                        MessageBox.Show("Restriction:\n" + s.restrict_type + "\n" + s.restrict_id);
                        return;
                }
            }
            JObject j = await GetWorldSongToken(songid);
            remainStamina = j["value"]["stamina"].Value<int>();
            string token = j["value"]["token"].Value<string>();
            JObject j2 = await SubmitScore(token, hash, songid, noteCount);
            JObject userInfo = await GetUserInfo();
            Invoke(new JObjectReceiver(UpdateUserInfo), userInfo);
            JObject mapInfo = await GetSpecificMap(currentMapId);
            WorldMapTree.Invoke(new JObjectReceiver(UpdateCurrentMap), mapInfo);
        }
        public async void ConvertStaminaAsync()
        {
            await ConvertFragmentStamina();
            JObject userInfo = await GetUserInfo();
            Invoke(new JObjectReceiver(UpdateUserInfo), userInfo);
        }
        public async void RefreshAsync()
        {
            JObject userInfo = await GetUserInfo();
            Invoke(new JObjectReceiver(UpdateUserInfo), userInfo);
        }
        public async void PlayNormal()
        {
            string songid = "onelastdrive";
            int noteCount = (await GetWorldRanking(songid))["value"][0]["perfect_count"].Value<int>();
            string hash = GetSongHash(songid);
            string token = await GetSongToken();
            JObject j2 = await SubmitScore(token, hash, songid, noteCount, 755);
            bool succeed = true;
        }

        public void UpdateWorldMapTree(JObject j)
        {
            WorldMapTree.BeginUpdate();

            SetCurrentMapId(j["value"]["current_map"].Value<string>());
            List<WorldMap> maps = new List<WorldMap>();
            foreach (var map in j["value"]["maps"])
            {
                maps.Add(map.ToObject<WorldMap>());
            }

            WorldMapTree.Nodes.Clear();
            worldMaps.Clear();

            try
            {
                List<(TreeNode, int)> roots = new List<(TreeNode, int)>();

                foreach (var m in maps)
                {
                    if (roots.Where((b) => b.Item2 == m.chapter).Count() == 0)
                    {
                        roots.Add((new TreeNode(m.chapter.ToString()), m.chapter));
                    }
                    var node = roots.Where((b) => b.Item2 == m.chapter).ToArray()[0].Item1.Nodes.Add(m.map_id);
                    worldMaps.Add(m.map_id, m);
                }

                roots.Sort(((TreeNode, int) a, (TreeNode, int) b) => a.Item2.CompareTo(b.Item2));
                foreach (var root in roots) WorldMapTree.Nodes.Add(root.Item1);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
                WorldMapTree.Nodes.Clear();
                worldMaps.Clear();
            }

            WorldMapTree.EndUpdate();
        }
        public void UpdateCurrentMap(JObject j)
        {
            WorldMap map = j["value"]["maps"][0].ToObject<WorldMap>();
            worldMaps[currentMapId] = map;

            StepList.BeginUpdate();

            StepList.Clear();

            try
            {
                List<Step> steps = map.steps.ToList();
                steps.Sort((Step a, Step b) => a.position.CompareTo(b.position));

                foreach (Step step in steps)
                {
                    StepList.Items.Add($"{step.position} {(map.curr_position == step.position ? step.capture - map.curr_capture : (map.curr_position > step.position ? 0 : step.capture)).ToString("f1")}/{step.capture.ToString("f1")}");
                }

                StepList.EnsureVisible(map.curr_position);
                StepList.Items[map.curr_position].Selected = true;
                StepList.Select();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
                StepList.Clear();
            }

            StepList.EndUpdate();

            TotalStep.Text = map.steps.Sum((s) => s.capture).ToString();
        }
        public void UpdateUserInfo(JObject j)
        {
            if (j["value"].Contains("curr_ts"))
            {
                long curr_ts = j["value"]["curr_ts"].Value<long>();
                long next_convert = j["value"]["next_fragstam_ts"].Value<long>();
                ConvertStamina.Enabled = curr_ts > next_convert;
                remainStamina = j["value"]["stamina"].Value<int>();
                Stamina.Text = remainStamina.ToString();
                Character.Text = j["value"]["character"].Value<int>().ToString();
            }
        }

        private async void OnWorldMapTreeSelect(object sender, TreeViewEventArgs e)
        {
            if (!worldMaps.ContainsKey(e.Node.Text)) return;
            WorldMap map = worldMaps[e.Node.Text];
            JObject ret = await SetCurrentMap(e.Node.Text);
            if (!ret["success"].Value<bool>()) return;
            SetCurrentMapId(e.Node.Text);
        }

        private async void AutoPlay()
        {
            while (true)
            {
                try
                {
                    string songid = "flyburg";
                    int noteCount = (await GetWorldRanking(songid))["value"][0]["perfect_count"].Value<int>();
                    string hash = GetSongHash(songid);
                    Step s = worldMaps[currentMapId].steps[worldMaps[currentMapId].curr_position];
                    if (s.restrict_type != null)
                    {
                        switch (s.restrict_type)
                        {
                            case "song_id":
                                songid = s.restrict_id;
                                hash = GetSongHash(songid);
                                noteCount = (await GetWorldRanking(songid))["value"][0]["perfect_count"].Value<int>();
                                break;
                            default:
                                MessageBox.Show("Restriction:\n" + s.restrict_type + "\n" + s.restrict_id);
                                return;
                        }
                    }
                    JObject j = await GetWorldSongToken(songid);
                    remainStamina = j["value"]["stamina"].Value<int>();
                    string token = j["value"]["token"].Value<string>();
                    JObject j2 = await SubmitScore(token, hash, songid, noteCount);
                    JObject userInfo = await GetUserInfo();
                    Invoke(new JObjectReceiver(UpdateUserInfo), userInfo);
                    JObject mapInfo = await GetSpecificMap(currentMapId);
                    WorldMapTree.Invoke(new JObjectReceiver(UpdateCurrentMap), mapInfo);
                }
                catch (Exception)
                {
                }
                Thread.Sleep(1000 * 60 * 60 * 2 + 3000);
            }
        }
        private void ConvertStamina_Click(object sender, EventArgs e)
        {
            ConvertStaminaAsync();
        }
        private void DoParty_Click(object sender, EventArgs e)
        {
            PlayParty();
        }
        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            RefreshAsync();
        }
    }
}
