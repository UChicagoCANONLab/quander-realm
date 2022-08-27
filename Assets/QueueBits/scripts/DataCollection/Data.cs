
using System;
using System.Collections.Generic;
public class Data
{
	public int level;
	public int winner; //0 = tie 1 = player 2 = AI 
						//maps position index to value
	public int prefilledBoard;
	public int[] placement_order; //even turns = player, odd turns = AI
	public int[] superposition;
	public int[] reveal_order; //even turns = player, odd turns = AI
	public int[] outcome; //1 = player 2 = AI
}

//using MySql.Data;
// **CHANGE TARGET FRAMEWORK TO 4.8**

//using System;

//namespace Data
//{
//    public class DBConnection
//    {
//        public DBConnection()
//        {
//        }

//        public string Server { get; set; }
//        public string DatabaseName { get; set; }
//        public string UserName { get; set; }
//        public string Password { get; set; }

//        public MySqlConnection Connection { get; set; }

//        public static DBConnection _instance = null;
//        public static DBConnection Instance()
//        {
//            if (_instance == null)
//                _instance = new DBConnection();
//            return _instance;
//        }

//        public bool IsConnect()
//        {
//            if (Connection == null)
//            {
//                if (String.IsNullOrEmpty(DatabaseName))
//                    return false;
//                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
//                Connection = new MySqlConnection(connstring);
//                Connection.Open();
//            }

//            return true;
//        }

//        public void Close()
//        {
//            Connection.Close();
//        }
//    }
//}