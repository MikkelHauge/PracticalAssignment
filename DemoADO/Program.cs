using DemoADO.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PracticalAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Programmet udskriver resultaterne fra begge stored procedures, og that's it. 
             * De 2 stored procedures er noteret som kommentarer i bunden af denne kode - fordi det var et nemt sted at finde dem, for dig, tænkte jeg.
             */

            Console.WriteLine("-------------------------------Starter-------------------------------");
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine();

            string connStr = @"Data Source=(localdb)\ProjectModels;Initial Catalog=Database1;Integrated Security=True;Pooling=False;Connect Timeout=30";
            SqlConnection con = new SqlConnection(connStr);

            List<User> Users = new List<User>();

            try
            {
                con.Open();
                // 2 Stored Procedures: 

                User user = GetUserDetails(1, con); // hvilket som helst tal (id) (der er kun resultater for 1 - 7, i databasen! sorry :D
                Console.WriteLine(user.ToString());


                List<string> Usernames = Nameswithletter('a', con); // hvilket som helst bogstav (der er kun resultater for a, b c og m, i databasen! sorry :D

                foreach (var name in Usernames)
                {
                    Console.WriteLine(name);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Fejl: " + ex.Message);
            }
            finally
            {
                // Luk forbindelsen
                con.Close();
            }

            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("-------------------------------Færdig--------------------------------");
            Console.ReadLine();
        }


        private static User GetUserDetails(int id, SqlConnection con)
        {
            User user = null;

            string sqlGetUserDetails = "Exec GetUserDetails @UserID = " + id;
            SqlCommand cmdSelect = new SqlCommand(sqlGetUserDetails, con);
            SqlDataReader reader = cmdSelect.ExecuteReader();

            while (reader.Read())
            {
                string name = reader["Username"].ToString();
                string email = reader["Email"].ToString();
                string address = reader["Address"].ToString();

                user = new User(name, email, address);
            }

            reader.Close();
            return user;
        }

        private static List<string> Nameswithletter(char a, SqlConnection con)
        {
            List<string> Usernames = new List<string>();
            string sqlGetUsersStartingWithA = "Exec getUsersStartingWithA @FirstLetter = '" + a + "'";
            SqlCommand cmdSelect2 = new SqlCommand(sqlGetUsersStartingWithA, con);

            SqlDataReader reader = cmdSelect2.ExecuteReader();

            while (reader.Read())
            {
                string name = reader["Username"].ToString();
                Usernames.Add(name);
            }
            reader.Close();
            return Usernames;
        }
    }
}
/*

CREATE   PROCEDURE GetUserDetails
    @UserID INT
AS
BEGIN
    IF @UserID IS NULL
    BEGIN
        RAISERROR('Invalid input.', 16, 1)
        RETURN
    END
    ELSE
    SELECT Username, Email, Address
    FROM Usernames U
    JOIN Emails E ON U.UserID = E.UserID
    JOIN Addresses A ON U.UserID = A.UserID
    WHERE U.UserID = @UserID
END




CREATE PROCEDURE GetUsersStartingWithA
	@FirstLetter char(1)
AS
BEGIN
    SELECT Username
    FROM Usernames
    WHERE Username LIKE @FirstLetter + '%'
END

*/