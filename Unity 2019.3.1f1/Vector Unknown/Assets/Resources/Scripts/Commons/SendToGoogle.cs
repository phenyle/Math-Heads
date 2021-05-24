using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleSheetsForUnity
{
    public class SendToGoogle : MonoBehaviour
    {
        //Player Holder
        public PlayerData player;

        //SearchValues
        private string searchTable;
        private string searchColumn;
        private string searchPlayer;


        private void OnEnable()
        {
            // Suscribe for catching cloud responses.
            Drive.responseCallback += HandleDriveResponse;
        }

        private void OnDisable()
        {
            // Remove listeners.
            Drive.responseCallback -= HandleDriveResponse;
        }


        public void CreatePlayer()
        {
            player = new PlayerData();
        }

        public void SaveNewPlayer()
        {
            string jsonPlayer = "";
            // Get the json string of the object.
            for (int i = 0; i < player.sheetNames.Length; i++)
            {
                jsonPlayer = "";
                switch(i)
                {
                    case 0: //users
                        jsonPlayer = JsonUtility.ToJson(player.users);
                        break;
                    case 1: //p1-1
                        jsonPlayer = JsonUtility.ToJson(player.p1_1);
                        break;
                    case 2: //p2-1
                        jsonPlayer = JsonUtility.ToJson(player.p2_1);
                        break;
                    case 3: //p2-2
                        jsonPlayer = JsonUtility.ToJson(player.p2_2);
                        break;
                    case 4: //p3-1
                        jsonPlayer = JsonUtility.ToJson(player.p3_1);
                        break;
                    case 5: //p4-1
                        jsonPlayer = JsonUtility.ToJson(player.p4_1);
                        break;
                    case 6: //p4-2
                        jsonPlayer = JsonUtility.ToJson(player.p4_2);
                        break;
                    case 7: //p4-3
                        jsonPlayer = JsonUtility.ToJson(player.p4_3);
                        break;
                }
                Debug.Log("<color=yellow>Sending following player table: " + player.sheetNames[i] + "\n</color>" + jsonPlayer);

                Drive.CreateObject(jsonPlayer, player.sheetNames[i], true);
            }
        }

        public void UpdatePlayer(bool create)
        {
            string jsonPlayer = "";
            // Get the json string of the object.
            for (int i = 0; i < player.sheetNames.Length; i++)
            {
                jsonPlayer = "";
                switch (i)
                {
                    case 0: //users
                        jsonPlayer = JsonUtility.ToJson(player.users);
                        break;
                    case 1: //p1-1
                        jsonPlayer = JsonUtility.ToJson(player.p1_1);
                        break;
                    case 2: //p2-1
                        jsonPlayer = JsonUtility.ToJson(player.p2_1);
                        break;
                    case 3: //p2-2
                        jsonPlayer = JsonUtility.ToJson(player.p2_2);
                        break;
                    case 4: //p3-1
                        jsonPlayer = JsonUtility.ToJson(player.p3_1);
                        break;
                    case 5: //p4-1
                        jsonPlayer = JsonUtility.ToJson(player.p4_1);
                        break;
                    case 6: //p4-2
                        jsonPlayer = JsonUtility.ToJson(player.p4_2);
                        break;
                    case 7: //p4-3
                        jsonPlayer = JsonUtility.ToJson(player.p4_3);
                        break;
                }
                Debug.Log("<color=yellow>Sending following player table: " + player.sheetNames[i] + "\n</color>" + jsonPlayer);

                // Look in the 'PlayerInfo' table, for an object of name as specified, and overwrite with the current obj data.
                Drive.UpdateObjects(player.sheetNames[i], "name", player.users.name, jsonPlayer, create, true);

            }
        }

        public void RetrievePlayerData(string tableName, string columnName, string playerName)
        {
            searchTable = tableName;
            searchColumn = columnName;
            searchPlayer = playerName;

            Debug.Log("<color=yellow>Retrieving from the worksheet table: </color>" + searchTable + "\n" +
                "<color=yellow>Under the column: </color>" + searchColumn + "\n" +
                "<color=yellow>Having the name: </color>" + searchPlayer);

            // Get any objects from table <searchTable> with value <searchPlayer> in the field called <searchColumn>.            

            Drive.GetObjectsByField(searchTable, searchColumn, searchPlayer, true);
        }

        public string RetrieveSalt(string username)
        {
            player.users.name = username;

            Debug.Log("<color=yellow>Retrieving player of name " + username + " from the Cloud.</color>");

            // Get any objects from table 'PlayerInfo' with value 'Mithrandir' in the field called 'name'.
            Drive.GetObjectsByField("users", "name", player.users.name, true);
            //
            return "fix this later";
        }

        public string RetrieveHash(string username)
        {
            player.users.name = username;

            Debug.Log("<color=yellow>Retrieving player of name " + username + " from the Cloud.</color>");

            // Get any objects from table 'PlayerInfo' with value 'Mithrandir' in the field called 'name'.
            Drive.GetObjectsByField("users", "name", player.users.name, true);

            return player.users.hash;
        }



        public void HandleDriveResponse(Drive.DataContainer dataContainer)
        {
            Debug.Log(dataContainer.msg);

            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getObjectsByField)
            {
                string rawJSon = dataContainer.payload;
                Debug.Log(rawJSon);

                switch(searchTable)
                {
                    case "users":
                        // Check if the type is correct.
                        if (string.Compare(dataContainer.objType, searchTable) == 0)
                        {
                            // Parse from json to the desired object type.
                            PlayerData.tbl_Users[] tmp_users = JsonHelper.ArrayFromJson<PlayerData.tbl_Users>(rawJSon);

                            for (int i = 0; i < tmp_users.Length; i++)
                            {
                                player.users = tmp_users[i];
                                Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>");
                            }
                        }
                        break;
                }

                /**
                //Walk through the tables on the Worksheet
                for(int i = 0; i < player.sheetNames.Length; i++)
                {
                    switch(i)
                    {
                        case 0: //users
                            // Parse from json to the desired object type.
                            PlayerData.tbl_Users[] user_datas = JsonHelper.ArrayFromJson<PlayerData.tbl_Users>(rawJSon);
                            for (int j = 0; j < user_datas.Length; j++)
                            {
                                player.users = user_datas[j];
                                Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" + player.sheetNames[i]);
                            }

                            break;
                        case 1: //p1-1
                                // Parse from json to the desired object type.
                            PlayerData.tbl_P1_1[] p1_1_datas = JsonHelper.ArrayFromJson<PlayerData.tbl_P1_1>(rawJSon);
                            for (int j = 0; j < p1_1_datas.Length; j++)
                            {
                                player.p1_1 = p1_1_datas[j];
                                Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" + player.sheetNames[i]);
                            }

                            break;
                        case 2: //p2-1
                                // Parse from json to the desired object type.
                            PlayerData.tbl_P2_1[] p2_1_datas = JsonHelper.ArrayFromJson<PlayerData.tbl_P2_1>(rawJSon);
                            for (int j = 0; j < p2_1_datas.Length; j++)
                            {
                                player.p2_1 = p2_1_datas[j];
                                Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" + player.sheetNames[i]);
                            }
                            break;
                        case 3: //p2-2
                                // Parse from json to the desired object type.
                            PlayerData.tbl_P2_2[] p2_2_datas = JsonHelper.ArrayFromJson<PlayerData.tbl_P2_2>(rawJSon);
                            for (int j = 0; j < p2_2_datas.Length; j++)
                            {
                                player.p2_2 = p2_2_datas[j];
                                Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" + player.sheetNames[i]);
                            }
                            break;
                        case 4: //p3-1
                                // Parse from json to the desired object type.
                            PlayerData.tbl_P3_1[] p3_1_datas = JsonHelper.ArrayFromJson<PlayerData.tbl_P3_1>(rawJSon);
                            for (int j = 0; j < p3_1_datas.Length; j++)
                            {
                                player.p3_1 = p3_1_datas[j];
                                Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" + player.sheetNames[i]);
                            }
                            break;
                        case 5: //p4-1
                                // Parse from json to the desired object type.
                            PlayerData.tbl_P4_1[] p4_1_datas = JsonHelper.ArrayFromJson<PlayerData.tbl_P4_1>(rawJSon);
                            for (int j = 0; j < p4_1_datas.Length; j++)
                            {
                                player.p4_1 = p4_1_datas[j];
                                Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" + player.sheetNames[i]);
                            }
                            break;
                        case 6: //p4-2
                                // Parse from json to the desired object type.
                            PlayerData.tbl_P4_2[] p4_2_datas = JsonHelper.ArrayFromJson<PlayerData.tbl_P4_2>(rawJSon);
                            for (int j = 0; j < p4_2_datas.Length; j++)
                            {
                                player.p4_2 = p4_2_datas[j];
                                Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" + player.sheetNames[i]);
                            }
                            break;
                        case 7: //p4-3
                                // Parse from json to the desired object type.
                            PlayerData.tbl_P4_3[] p4_3_datas = JsonHelper.ArrayFromJson<PlayerData.tbl_P4_3>(rawJSon);
                            for (int j = 0; j < p4_3_datas.Length; j++)
                            {
                                player.p4_3 = p4_3_datas[j];
                                Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" + player.sheetNames[i]);
                            }
                            break;

                    }
                
                }
**/
            }
/**
            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getTable)
            {
                string rawJSon = dataContainer.payload;
                Debug.Log(rawJSon);

                // Check if the type is correct.
                if (string.Compare(dataContainer.objType, "users") == 0)
                {
                    // Parse from json to the desired object type.
                    Player[] players = JsonHelper.ArrayFromJson<Player>(rawJSon);

                    string logMsg = "<color=yellow>" + players.Length.ToString() + " objects retrieved from the cloud and parsed:</color>";
                    for (int i = 0; i < players.Length; i++)
                    {
                        logMsg += "\n" +
                            "<color=blue>Name: " + players[i].name + "</color>\n" +
                            "Hash: " + players[i].hash + "\n" +
                            "Salt: " + players[i].salt + "\n" +
                            "Education: " + players[i].education + "\n" +
                            "Total Time: " + players[i].tot_time + "\n";
                    }
                    Debug.Log(logMsg);
                }
            }

            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getAllTables)
            {
                string rawJSon = dataContainer.payload;

                // The response for this query is a json list of objects that hold tow fields:
                // * objType: the table name (we use for identifying the type).
                // * payload: the contents of the table in json format.
                Drive.DataContainer[] tables = JsonHelper.ArrayFromJson<Drive.DataContainer>(rawJSon);

                // Once we get the list of tables, we could use the objTypes to know the type and convert json to specific objects.
                // On this example, we will just dump all content to the console, sorted by table name.
                string logMsg = "<color=yellow>All data tables retrieved from the cloud.\n</color>";
                for (int i = 0; i < tables.Length; i++)
                {
                    logMsg += "\n<color=blue>Table Name: " + tables[i].objType + "</color>\n" + tables[i].payload + "\n";
                }
                Debug.Log(logMsg);
            }
**/
        }
    }

}