using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Npgsql;


namespace PPB
{
    public static class DBHandler
    {

        public static NpgsqlConnection Connect()
        {
            var cs = "Server=localhost;Port=5432;Database=PPB;User Id=postgres;Password=1234;";
                
            var con = new NpgsqlConnection(cs);
            con.Open();

            return con;
        }

        public static bool Register(string username, string password)
        {
            try
            {


                var con = Connect();
                // hash password 
                // steps:(hasher: SHA256 Algorithmus, Encoding.UTF8.GetBytes(password): convert to byte array, hasher.ComputeHash: hash value for byte array, convert.ToBase64String: convert hash to string)
                var hasher = SHA256.Create();  
                string hashpassword = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(password)));

                var sql = "INSERT INTO users (id, username, password, name, bio, image, tournamentpoints, wins, losses, draws, gamesplayed) VALUES (@id, @username, @password, '', '', '', 100, 0, 0, 0, 0)";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashpassword);


                cmd.ExecuteNonQuery();

                PPB.AuthDict[username+"-ppbToken"] = GetUser(username); // authorization token ("username"-ppbToken)

                
                return true;

            } catch
            {
                return false;
            }           
        }

        public static bool Login(string username, string password)
        {
            try
            {
                var con = Connect();

                // hash password 
                // steps:(hasher: SHA256 Algorithmus, Encoding.UTF8.GetBytes(password): convert to byte array, hasher.ComputeHash: hash value for byte array, convert.ToBase64String: convert hash to string)
                var hasher = SHA256.Create();
                string hashpassword = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(password)));
                var sql = "SELECT username FROM users WHERE username= @username AND password= @hashpassword";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@hashpassword", hashpassword);
    
                object user = cmd.ExecuteScalar();


                if (user != null) // Username or Password doesn't exist
                {

                    PPB.AuthDict[username + "-ppbToken"] = GetUser(username); // authorization token ("username"-ppbToken)
                    return true;
                }
                else return false;

            }
            catch
            {
                return false;
            }
        }

        public static Guid StoreMultiMediaContent(MultiMediaContent mmc, Guid userid)
        {
            try
            {


                var con = Connect();

                var sql = "INSERT INTO multimediacontent (id, name, filetype, filesize, title, artist, album, rating, genre, length, url) VALUES (@Id, @Name, @Filetype, @Filesize, @Title, @Artist, @Album, @Rating, @Genre, @Length, @Url)";
                using var cmd = new NpgsqlCommand(sql, con);
                Guid mmcId = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@Id", mmcId);
                cmd.Parameters.AddWithValue("@Name", mmc.Name);
                cmd.Parameters.AddWithValue("@Filetype", mmc.Filetype);
                cmd.Parameters.AddWithValue("@Filesize", mmc.Filesize);
                cmd.Parameters.AddWithValue("@Title", mmc.Title);
                cmd.Parameters.AddWithValue("@Artist", mmc.Artist);
                cmd.Parameters.AddWithValue("@Album", mmc.Album);
                cmd.Parameters.AddWithValue("@Rating", mmc.Rating);
                cmd.Parameters.AddWithValue("@Genre", mmc.Genre);
                cmd.Parameters.AddWithValue("@Length", mmc.Length);
                cmd.Parameters.AddWithValue("@Url", mmc.Url);

                cmd.ExecuteNonQuery();

                StoreIDs(mmcId, userid);

                return mmcId;

            }
            catch
            {
                return Guid.Empty;
            }
        }

        public static void StoreIDs(Guid mmcId, Guid userId)
        {
            try
            {


                var con = Connect();

                var sql = "INSERT INTO multimediacontentusers (userid, mmcid) VALUES (@userid, @mmcid)";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@userid", userId);
                cmd.Parameters.AddWithValue("@mmcid", mmcId);

                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        public static bool DeleteMultiMediaContent(string name)
        {
            try
            {


                var con = Connect();
                Guid MmcId;
                var sql = "SELECT id FROM multimediacontent WHERE name= @Name";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    using var MmcIdReader = cmd.ExecuteReader();
                    MmcIdReader.Read();
                    MmcId = MmcIdReader.GetGuid(0);
                }

                sql = "DELETE FROM multimediacontent WHERE id= @Id";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", MmcId);
                    cmd.ExecuteNonQuery();
                }

                sql = "DELETE FROM multimediacontentusers WHERE mmcid= @Id";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", MmcId);
                    cmd.ExecuteNonQuery();
                }


                return true;

            }
            catch
            {
                return false;
            }
        }

        public static List<User> GetAllUsers()
        {
            var con = Connect();
            var sql = "SELECT username, password, name, bio, image, tournamentpoints, wins, losses, draws, gamesplayed FROM users";
            using var cmd = new NpgsqlCommand(sql, con);

            using var UsersReader = cmd.ExecuteReader();
            List<User> Users = new List<User>();

            while (UsersReader.Read()) 
            {   
                var Username = UsersReader.GetString(0);
                var Password = UsersReader.GetString(1);
                var Name = UsersReader.GetString(2);
                var Bio = UsersReader.GetString(3);
                var Image = UsersReader.GetString(4);
                var TournamentPoints = UsersReader.GetInt32(5);
                var Wins = UsersReader.GetInt32(6);
                var Losses = UsersReader.GetInt32(7);
                var Draws = UsersReader.GetInt32(8);
                var GamesPlayed = UsersReader.GetInt32(9);

                var User = new User();
                User.Username = Username;
                User.Password = Password;
                User.Name = Name;
                User.Bio = Bio;
                User.Image = Image;
                User.TournamentPoints = TournamentPoints;
                User.Wins = Wins;
                User.Losses = Losses;
                User.Draws = Draws;
                User.GamesPlayed = GamesPlayed;

                Users.Add(User);
            }

            return Users;
        }

        public static User GetUser(string username)
        {

            try
            {
                var con = Connect();
                var sql = "SELECT id, username, password, name, bio, image, tournamentpoints, wins, losses, draws, gamesplayed FROM users WHERE username= @Username";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Username", username);

                using var UsersReader = cmd.ExecuteReader();

                UsersReader.Read();

                var UserId = UsersReader.GetGuid(0);
                var Username = UsersReader.GetString(1);
                var Password = UsersReader.GetString(2);
                var Name = UsersReader.GetString(3);
                var Bio = UsersReader.GetString(4);
                var Image = UsersReader.GetString(5);
                var TournamentPoints = UsersReader.GetInt32(6);
                var Wins = UsersReader.GetInt32(7);
                var Losses = UsersReader.GetInt32(8);
                var Draws = UsersReader.GetInt32(9);
                var GamesPlayed = UsersReader.GetInt32(10);

                var User = new User();
                User.UserId = UserId;
                User.Username = Username;
                User.Password = Password;
                User.Name = Name;
                User.Bio = Bio;
                User.Image = Image;
                User.TournamentPoints = TournamentPoints;
                User.Wins = Wins;
                User.Losses = Losses;
                User.Draws = Draws;
                User.GamesPlayed = GamesPlayed;

                return User;
            }
            catch
            {
                return null;
            }
        }

        public static void UpdateStats(User user)
        {
            try
            {
                var con = Connect();
                var sql = "UPDATE users SET tournamentpoints= @TournamentPoints, wins= @Wins, losses= @Losses, draws= @Draws, gamesplayed= @GamesPlayed WHERE id= @Id";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@TournamentPoints", user.TournamentPoints);
                cmd.Parameters.AddWithValue("@Wins", user.Wins);
                cmd.Parameters.AddWithValue("@Losses", user.Losses);
                cmd.Parameters.AddWithValue("@Draws", user.Draws);
                cmd.Parameters.AddWithValue("@GamesPlayed", user.GamesPlayed);
                cmd.Parameters.AddWithValue("@Id", user.UserId);

                cmd.ExecuteNonQuery();
                
                

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        public static void UpdateProfile(User user)
        {
            try
            {
                var con = Connect();
                var sql = "UPDATE users SET name= @Name, bio= @Bio, image= @Image WHERE id= @Id";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Bio", user.Bio);
                cmd.Parameters.AddWithValue("@Image", user.Image);
                cmd.Parameters.AddWithValue("@Id", user.UserId);

                cmd.ExecuteNonQuery();



            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        public static bool ChangeUsername(string currentUsername, string newUsername)
        {
            try
            {
                var con = Connect();
                var sql = "UPDATE users SET username= @newUsername WHERE username= @currentUsername";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@newUsername", newUsername);
                cmd.Parameters.AddWithValue("@currentUsername", currentUsername);

                cmd.ExecuteNonQuery();


                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ChangePassword(string username, string oldPassword, string newPassword)
        {

            try
            {
                var con = Connect();

                // hash password 
                // steps:(hasher: SHA256 Algorithmus, Encoding.UTF8.GetBytes(password): convert to byte array, hasher.ComputeHash: hash value for byte array, convert.ToBase64String: convert hash to string)
                var hasher = SHA256.Create();
                string hashOldpassword = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(oldPassword)));
                string hashNewpassword = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(newPassword)));
                var sql = "UPDATE users SET password= @newPassword WHERE username=  @username AND password= @oldPassword";
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@newPassword", hashNewpassword);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@oldPassword", hashOldpassword);

                cmd.ExecuteNonQuery();

                
                return true;
            }
            catch
            {      
                return false;
            }
        }
    }
}
