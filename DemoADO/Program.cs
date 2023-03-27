using DemoADO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PracticalAssignment
{
    class Program
    {
        private static readonly string connectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=Database1;Integrated Security=True;Pooling=False;Connect Timeout=30";
        private static SqlConnection con = new SqlConnection(connectionString);
        private static List<String> Users = new List<String>();
        private static User user = null;

        static void Main(string[] args)
        {
            con.Open();
            RunProgram();
            con.Close();
        }

        public static void RunProgram()
        {
            Console.WriteLine("-------------------------------Starter-------------------------------");
            Console.WriteLine();

            Console.WriteLine("Vælg eller skriv navnet på en stored procedure: (GetUsernamesStartingWithX, GetUserDetails)");
            Console.WriteLine("1: GetUsernamesStartingWithX");
            Console.WriteLine("2: GetUserDetails");

            AskForInput();

            Console.WriteLine();
            Console.WriteLine("-------------------------------Færdig--------------------------------");
            Console.ReadLine();
        }

        private static void AskForInput()
        {
            string userInput = Console.ReadLine();

            if (userInput.ToLower() == "getusernamesstartingwithx" || userInput == "1")
            {
                Console.WriteLine("Hent alle brugere ud fra forbogstav");

                GetUsersFromFirstLetter();

            }
            else if (userInput.ToLower() == "getuserdetails" || userInput == "2")
            {
                Console.WriteLine("Hent en bruger ud fra ID");
                InputUserID();
            }
            else
            {
                Console.WriteLine("Forkert input - prøv indtastningen igen");
                AskForInput();
            }
        }



        private static void GetUsersFromFirstLetter()
        {
            string letter;
            do
            {
                Console.WriteLine("Indtast forbogstav (eller skriv 'exit' for at stoppe): ");
                Console.WriteLine("NB: Den prøver at hente brugere, med det første bogstav i den string du skriver.");

                letter = Console.ReadLine();

                if (letter.ToLower() == "exit")
                {
                    RunProgram();
                }

                Users = GetUsernamesStartingWithX(letter);

                if (Users.Count == 0)
                    Console.WriteLine($"Ingen brugere med '{letter}' som forbogstav blev fundet. Prøv et andet!");
                else
                {
                    Console.WriteLine($"Der er fundet følgende brugere, med forbogstav: '{letter}':");
                    foreach (string username in Users)
                    {
                        Console.WriteLine(username);
                    }
                }

            } while (true);
        }

        private static List<string> GetUsernamesStartingWithX(string letter)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sqlGetUsernames = "GetUsernamesStartingWithX";

                using (SqlCommand cmd = new SqlCommand(sqlGetUsernames, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartLetter", letter);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string username = reader.GetString(0);
                            Users.Add(username);
                        }
                    }
                }
            }
            return Users;
        }
        private static void InputUserID()
        {
            Console.WriteLine("Indtast ID (Fx. BBA34C89-4D6E-426B-B6F6-03BA6DF65B93 ) ");
            string inputUserID = Console.ReadLine();
            FetchUser(inputUserID);
        }

        private static User FetchUser(string inputUserID)
        {
            Guid userId;
            if (Guid.TryParse(inputUserID, out userId)) // string til GUID
            {
                user = GetUserDetails(userId);
                if (user != null)
                {
                    Console.WriteLine(user.ToString());
                    
                }
                else
                {
                    Console.WriteLine("User ikke fundet.");
                }
                return user;
            }
            else
            {
                Console.WriteLine("Ugyldigt ID. Prøv igen.");
                InputUserID(); // spørg efter ID igen.
                return null;
            }
        }

        private static User GetUserDetails(Guid userId)
        {
            User user = null;
            string execStoredProcedure = "EXEC GetUserDetails @UserId";
            SqlCommand cmdSelect = new SqlCommand(execStoredProcedure, con);
            cmdSelect.Parameters.AddWithValue("@UserId", userId);

            SqlDataReader reader = cmdSelect.ExecuteReader();

            while (reader.Read())
            {
                string name = reader["Username"].ToString();
                string email = reader["Email"].ToString();
                string address = reader["Address"].ToString();
                user = new User(name, email, address, userId);
            }

            reader.Close();
            return user;
        }
    }
}
