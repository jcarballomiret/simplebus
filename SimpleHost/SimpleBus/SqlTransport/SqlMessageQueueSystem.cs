using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using SimpleBus.Helpers;

namespace SimpleBus.SqlTransport
{
    public class SqlMessageQueueSystem : IMessageQueueSystem
    {
        public string TransportConnectionString { get; set; }

        public QueueMessage PopMessage(string queueName)
        {
            using (var connection = new SqlConnection(TransportConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"delete top (1) {queueName} output deleted.* ", connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var transportMessage = new QueueMessage()
                        {
                            Id = reader["id"].ToString(),
                            SentDateTime = (DateTime)reader["sent_on"],
                            Type = reader["type"].ToString(),
                            Headers = reader["header"].ToString(),
                            Body = JsonConvert.DeserializeObject(reader["body"].ToString(), TypeHelper.GetType(reader["type"].ToString()))
                        };

                        return transportMessage;
                    }
                }
            }
            return null;
        }

        public void InsertMessage(QueueMessage message, string queueName)
        {
            InsertMessage(message, queueName, 0);
        }

        private void InsertMessage(QueueMessage message, string queueName, int tryNumber)
        {
            
            using (
                var mycon = new SqlConnection())
            {
                mycon.ConnectionString = TransportConnectionString;

                var mycomm = new SqlCommand
                {
                    CommandType = CommandType.Text,
                    Connection = mycon,
                    CommandText =
                        $@"
                                        INSERT INTO [dbo].[{queueName}]
                                               ([id]
                                               ,[sent_on]
                                               ,[type]
                                               ,[header]
                                               ,[body])
                                         VALUES
                                               ('{
                            message.Id}', '{message.SentDateTime.ToString("yyyy-MM-dd hh:mm:ss.mmm")}', '{message.Type
                            }', '{message.Headers}', '{message.Body}')"
                };

                try
                {
                    mycon.Open();

                    mycomm.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    if (tryNumber > 5)
                    {
                        // TODO: log
                        throw;
                    }
                    InsertMessage(message, queueName, tryNumber + 1);
                }
            }
        }

        public void RollbackMessagePop(QueueMessage message)
        {
            throw new System.NotImplementedException();
        }


        public void CreateQueueIfDoesNotExist(string queueName)
        {
            using (
                var mycon = new SqlConnection())
            {
                mycon.ConnectionString = TransportConnectionString;

                var mycomm = new SqlCommand
                {
                    CommandType = CommandType.Text,
                    Connection = mycon,
                    CommandText = string.Format(@"
                                        if not exists (select * from sysobjects where name='{0}')
                                        create table {0}
                                        (
                                            id uniqueidentifier,
                                            sent_on datetime,
                                            type nvarchar(255),
                                            header nvarchar(max),
                                            body nvarchar(max),
                                            primary key (id)
                                        );", queueName)
                };

                mycon.Open();

                mycomm.ExecuteNonQuery();

            }
        }
    }
}
