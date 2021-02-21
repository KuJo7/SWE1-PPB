using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PPB
{
    public static class PPB
    {
        public static Dictionary<string, User> AuthDict = new Dictionary<string, User>(); // registered Users
        private static DateTime starttime = new DateTime(2021, 1, 1, 0, 0, 0); // starttime of a tournament
        private static User currentAdmin = null; // current Administrator
        private static object lockObj = new object(); // lock battle request
        public static List<User> OnlineUsers { get; set; } = new List<User>(); // users participating in a tournament
        public static List<MultiMediaContent> Playlist { get; set; } = new List<MultiMediaContent>(); // global playlist

        public static void Run()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 10001);
            listener.Start(5);

            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings() { Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() }, Formatting = Formatting.Indented};
            while (true)
            {

                Console.WriteLine("Waiting for a request...");
                var socket = listener.AcceptTcpClient();
                new Thread(() => RunRequest(socket)).Start();
            }
        }

        public static void RunRequest(TcpClient socket)
        {
            try
            {
                Console.WriteLine("Request recieved!");
                using var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
                using var reader = new StreamReader(socket.GetStream());

                // Streamreader
                string firstline = reader.ReadLine(); // firstline of header
                string httpverb = firstline.Split(" ")[0]; // split firstline for the httpverb
                string resource = firstline.Split(" ")[1]; // split firstline for the resource
                string httpversion = firstline.Split(" ")[2]; // split firstline for the httpversion
                string header;
                string authorization = "";
                int contentLength = 0;
                string body;
                Dictionary<string, string> headerDict = new Dictionary<string, string>();  // store request headers

                while (true)
                {
                    header = reader.ReadLine(); // store header

                    if (header == "")
                        break;

                    headerDict[header.Split(": ")[0]] = header.Split(": ")[1]; // split request headers [key] [value] 
                    if (header.StartsWith("Authorization:"))
                    {
                        authorization = header.Substring(21); // store authorization token
                    }
                    if (header.StartsWith("Content-Length:"))
                    {
                        contentLength = Convert.ToInt32(header.Substring(16)); // convert to int and store in contentLength
                    }
                }

                // store body
                char[] bodyBuffer = new char[contentLength];
                reader.ReadBlock(bodyBuffer);
                body = new string(bodyBuffer);

                // instantiate object context
                RequestContext context = new RequestContext(httpverb, resource, httpversion, headerDict, authorization, body);

                // output message
                Console.WriteLine(httpverb + " " + resource + " " + httpversion);

                // calling ProcessRequest() with object parameter 
                // store return values into two variables
                var (returnStatusCode, returncontent) = ProcessRequest(context);


                // Streamwriter
                // httpversion                //statusint                  //statusmessage
                writer.WriteLine("HTTP/1.1 " + Convert.ToInt32(returnStatusCode) + " " + returnStatusCode);
                writer.WriteLine("Content-Length: " + returncontent.Length);
                writer.WriteLine();
                if (returncontent.Length > 0)
                    writer.Write(returncontent);
                writer.Flush(); // send immediately

            }
            catch (Exception exc)
            {
                Console.WriteLine("error occurred: " + exc.Message);
            }
        }

        public static (HttpStatusCode returnstatusCode, string returnBody) ProcessRequest(RequestContext context)
        {
            string returncontent = "error"; // error by default
            System.Net.HttpStatusCode returnStatusCode = HttpStatusCode.NotFound; // not found by default

            if (context.Httpverb == "POST" && context.Resource == "/users") // register a user
            {
                var userCreds = JsonConvert.DeserializeObject<UserCredentials>(context.Body);
                
                if (DBHandler.Register(userCreds.Username, userCreds.Password))
                {
                    returncontent = JsonConvert.SerializeObject( userCreds.Username + " is registered");
                    returnStatusCode = HttpStatusCode.Created;
                    Console.WriteLine(userCreds.Username + " is registered");
                }

            }
            else if (context.Httpverb == "POST" && context.Resource == "/sessions") // login a user
            {
                var userCreds = JsonConvert.DeserializeObject<UserCredentials>(context.Body);
                if (DBHandler.Login(userCreds.Username, userCreds.Password))
                {
                    returncontent = JsonConvert.SerializeObject(userCreds.Username + " is logged in");
                    returnStatusCode = HttpStatusCode.OK;
                    Console.WriteLine(userCreds.Username + " is logged in");
                }
            }
            else if (context.Httpverb == "GET" && context.Resource == "/playlist") // get playlist
            {
                if (Playlist.Count == 0)
                {
                    returncontent = JsonConvert.SerializeObject("playlist is empty");
                    returnStatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                returncontent = JsonConvert.SerializeObject(Playlist);
                returnStatusCode = HttpStatusCode.OK;
                Console.WriteLine("Playlist displayed");
                }
            }

            else if (CheckAuthorization(context.Authorization, out var user)) // check authorization token
            {
                if (context.Httpverb == "GET" && context.Resource == "/users/" + user.Username) // show a user
                {

                    returncontent = JsonConvert.SerializeObject(new { Name = user.Name, Bio = user.Bio, Image = user.Image});
                    returnStatusCode = HttpStatusCode.OK;
                    Console.WriteLine("Profile of " + user.Username + " displayed");
                }

                if (context.Httpverb == "PUT" && context.Resource == "/users/" + user.Username) // update a user
                {
                    User updatedUser = JsonConvert.DeserializeObject<User>(context.Body);
                    updatedUser.UserId = user.UserId;
                    DBHandler.UpdateProfile(updatedUser);
                    user.Name = updatedUser.Name;
                    user.Bio = updatedUser.Bio;
                    user.Image = updatedUser.Image;
                    returncontent = JsonConvert.SerializeObject("Profile of " + user.Username + " updated");
                    returnStatusCode = HttpStatusCode.OK;
                    Console.WriteLine("Profile of " + user.Username + " updated");
                }

                if (context.Httpverb == "GET" && context.Resource == "/stats") // get user stats
                {

                    returncontent = JsonConvert.SerializeObject(new {Username = user.Username, TournamentPoints = user.TournamentPoints, Rank= user.Rank, Wins = user.Wins, Losses = user.Losses, Draws = user.Draws, GamesPlayed = user.GamesPlayed});
                    returnStatusCode = HttpStatusCode.OK;
                    Console.WriteLine("Stats of " + user.Username + " displayed");
                }

                if (context.Httpverb == "GET" && context.Resource == "/score") // get user scoreboard
                {
                    var userscores = Scoreboard.GetUserswithMostWins();
                    returncontent = JsonConvert.SerializeObject(userscores.Select(user => new {Username = user.Username, Wins = user.Wins }), Formatting.None);
                    returnStatusCode = HttpStatusCode.OK;
                    Console.WriteLine("Scoreboard of " + user.Username + " displayed");
                }

                if (context.Httpverb == "GET" && context.Resource == "/lib") // get user library
                {

                    if (user.Library.Count == 0)
                    returncontent = JsonConvert.SerializeObject("library is empty");
                    else
                    returncontent = JsonConvert.SerializeObject(user.Library);
                    returnStatusCode = HttpStatusCode.OK;
                    Console.WriteLine("Library of " + user.Username + " displayed");
                }

                if (context.Httpverb == "POST" && context.Resource == "/lib") // add Multi-media-content to user library
                {
                    var mmc = new MultiMediaContent();
                    mmc = JsonConvert.DeserializeObject<MultiMediaContent>(context.Body);
                    mmc.ContentId = DBHandler.StoreMultiMediaContent(mmc, user.UserId);
                    user.Library.Add(mmc);
                    returncontent = JsonConvert.SerializeObject(mmc.Name + " added to the library of " + user.Username);
                    returnStatusCode = HttpStatusCode.OK;
                    Console.WriteLine(mmc.Name + " added to the library of " + user.Username);
                }

                if (context.Httpverb == "DELETE" && context.Resource.StartsWith("/lib/")) // add Multi-media-content to user library
                {
                    string mmcname = context.Resource.Substring(5);
                    if (user.Library.Any(m => m.Name == mmcname)) // check if song is in the library
                    {
                        if (DBHandler.DeleteMultiMediaContent(mmcname))
                        {
                            for (int i = 0; i < user.Library.Count; i++)
                            {
                                if (user.Library[i].Name == mmcname)
                                {
                                    user.Library.RemoveAt(i);
                                    break; // break if the song is deleted
                                }

                            }
                            for (int i = 0; i < Playlist.Count; i++)
                            {
                                if (Playlist[i].Name == mmcname)
                                {
                                    Playlist.RemoveAt(i);
                                    i--; // to avoid skipping songs
                                }
                            }
                            returncontent = JsonConvert.SerializeObject(mmcname + " removed from the library of " + user.Username);
                            returnStatusCode = HttpStatusCode.OK;
                            Console.WriteLine(mmcname + " removed from the library of " + user.Username);
                        }
                    }// else error (not authorized)
                } 

                if (context.Httpverb == "POST" && context.Resource == "/playlist") // add Multi-media-content to playlist
                {
                    var mmcname = JsonConvert.DeserializeObject<MultiMediaContentName>(context.Body);
                    if (user.Library.Any(m => m.Name == mmcname.Name)) // check if the song belongs to user
                    {

                    Playlist.Add(user.Library.First(m => m.Name == mmcname.Name));
                    returncontent = JsonConvert.SerializeObject(mmcname.Name + " added to the playlist");
                    returnStatusCode = HttpStatusCode.OK;
                    Console.WriteLine(mmcname.Name + " added to the playlist");
                    } // else error (not authorized)

                }

                if (context.Httpverb == "PUT" && context.Resource == "/playlist" && user == currentAdmin) // reorder playlist
                {
                    var positions = JsonConvert.DeserializeObject<PositionChange>(context.Body);
                    var mmc = Playlist[positions.FromPosition]; // swap positions with a temp variable
                    Playlist[positions.FromPosition] = Playlist[positions.ToPosition];
                    Playlist[positions.ToPosition] = mmc;
                    int from = positions.FromPosition + 1;
                    int to = positions.ToPosition + 1;
                    returncontent = JsonConvert.SerializeObject("Playlist reordered from Position: " + from + " to Position: " + to);
                    returnStatusCode = HttpStatusCode.OK;
                    Console.WriteLine("Playlist reordered");
                }

                if (context.Httpverb == "GET" && context.Resource == "/actions") // get actions
                {
                    if (user.Actions.Count == 0) 
                    { 
                        returncontent = JsonConvert.SerializeObject("actions not defined");
                        returnStatusCode = HttpStatusCode.NotFound;
                        Console.WriteLine("actions not defined");
                    }
                    else
                    {
                        returncontent = JsonConvert.SerializeObject(user.Actions, Formatting.None);
                        returnStatusCode = HttpStatusCode.OK;
                        Console.WriteLine("actions displayed");
                    }
                }

                if (context.Httpverb == "PUT" && context.Resource == "/actions") // update actions
                {
                    var actionstr = JsonConvert.DeserializeObject<ActionString>(context.Body);
                    if (actionstr.Actions.Length == 5)
                    {
                        try
                        {
                            var Actions = new List<Actions>();
                            foreach (var ch in actionstr.Actions)
                            {
                                Actions.Add((Actions)Enum.Parse(typeof(Actions), ch.ToString())); // add new Actions
                            }
                            user.Actions = Actions; // replace old list with new list
                            returncontent = JsonConvert.SerializeObject(user.Username + " set actions");
                            returnStatusCode = HttpStatusCode.OK;
                            Console.WriteLine(user.Username + " set actions");
                        }
                        catch (Exception e)
                        {
                            returncontent = "error";
                            returnStatusCode = HttpStatusCode.NotFound;
                            Console.WriteLine(e.Message);
                        }
                    } // else error (not 5 actions)
                }

                if (context.Httpverb == "POST" && context.Resource == "/battles") // battles
                {
                    DateTime requesttime = DateTime.Now.AddSeconds(-15);
                    try
                    {
                        lock (lockObj) 
                        {
                            if (starttime < requesttime) // first user starts the tournament
                            {
                                starttime = DateTime.Now;
                                OnlineUsers.Clear();
                                OnlineUsers.Add(user);
                                returncontent = JsonConvert.SerializeObject(user.Username + " started the tournament");
                                returnStatusCode = HttpStatusCode.OK;
                                Console.WriteLine(user.Username + " started the tournament");
                                new Thread(UpdateBattleStats).Start(); // starts a thread to update BattleStats
                            }
                            else // user joins running tournament
                            {
                                if (OnlineUsers.Contains(user)) // user cannot play against himself
                                {
                                    returncontent = JsonConvert.SerializeObject(user.Username + " is already in the tournament");
                                    returnStatusCode = HttpStatusCode.NotFound;
                                    Console.WriteLine(user.Username + " is already in the tournament");

                                }
                                else
                                {
                                    var alllogs = new List<string>();
                                    foreach (var opponent in OnlineUsers)
                                    {
                                        if (opponent.IsBlocked == true) // Blocked players cannot participate in the current tournament
                                            continue;
                                        Console.WriteLine("Battle: " + opponent.Username + " -> " + user.Username);
                                        var (winner, loser, log) = BattleHandler.Battle(opponent, user);
                                        alllogs.AddRange(log); // concatenate strings
                                        alllogs.Add("----------------------------------------------------------------------------------------------------");

                                        if (winner == null && loser == null) // blocked for tournament if draw
                                        {
                                            user.IsBlocked = true;
                                            opponent.IsBlocked = true;
                                        }
                                        else // one winner and one loser
                                        {
                                            winner.IsBlocked = false;

                                            loser.IsBlocked = false;
                                        }
                                    }

                                    returncontent = JsonConvert.SerializeObject(alllogs);
                                    returnStatusCode = HttpStatusCode.OK;
                                    
                                    OnlineUsers.Add(user);
                                    var bestPlayers = OnlineUsers.OrderByDescending(u => u.BattlePoints).ToList(); // sort players after hightest battlepoints

                                    if (bestPlayers[0].BattlePoints != bestPlayers[1].BattlePoints) // if the first two players have not equal battlepoints -> admin = new admin, else -> admin = oldadmin
                                        currentAdmin = bestPlayers[0];
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        returncontent = "error";
                        returnStatusCode = HttpStatusCode.NotFound;
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return (returnStatusCode, returncontent);
        }

        private static bool CheckAuthorization(string authorization, out User user) // User object from AuthDict with -ppbToken
        {
            if (AuthDict.TryGetValue(authorization, out user)) return true;
            else return false;
        }

        private static void UpdateBattleStats()
        {

            Thread.Sleep(15000); // update Stats when tournament ends (after 15 seconds)
            
            foreach (var player in OnlineUsers)
            {
                if (player.IsBlocked == true) // player blocked for the tournament
                {
                    player.Draws++;
                    player.GamesPlayed++;
                    DBHandler.UpdateStats(player);
                    player.IsBlocked = false; // unblock player after the tournament
                    continue;
                }
                    
                if (player != currentAdmin) // player lost tournament
                {
                    player.TournamentPoints--;
                    player.Losses++;
                    player.GamesPlayed++;
                }
                else // player won tournament
                {
                    player.TournamentPoints++;
                    player.Wins++;
                    player.GamesPlayed++;
                }

                DBHandler.UpdateStats(player);
            }
        }
    }
}
