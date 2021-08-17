using AceQL.Client.Api;
using AceQL.Client.Test.Connection;
using AceQL.Client.Test.Dml;
using AceQL.Client.Test.Util;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AceQL.Client.test.Dml
{
    /// <summary>
    /// Class DmlSequenceTest.
    /// Do a full sequence of INSERT / SELECT / UPDATE / SELECT and test at each
    /// action that attended values are OK with Xunit.
    /// </summary>
    public class DmlSequenceTest
    {
        private AceQLConnection connection;

        public static void TheMain(string[] args)
        {
            try
            {
                DoIt().Wait();

                AceQLConsole.WriteLine();
                AceQLConsole.WriteLine("Press enter to continue....");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                AceQLConsole.WriteLine(exception.ToString());
                AceQLConsole.WriteLine(exception.StackTrace);
                AceQLConsole.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }

        static async Task DoIt()
        {

            var netCoreVer = System.Environment.Version; // 3.0.0
            AceQLConsole.WriteLine(netCoreVer + "");

            using (AceQLConnection connection = await ConnectionCreator.ConnectionCreateAsync().ConfigureAwait(false))
            {
                DmlSequenceTest dmlSequenceTest = new DmlSequenceTest(connection);
                await dmlSequenceTest.TestSequence();
            }

        }

        public DmlSequenceTest(AceQLConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Do a full sequence of INSERT / SELECT / UPDATE / SELECT and test at each
        /// action that attended values are OK with Xunit.
        /// </summary>
        public async Task TestSequence()
        {
            // Purge all
            SqlDeleteTest sqlDeleteTest = new SqlDeleteTest(connection);
            await sqlDeleteTest.DeleteOrderlogAll();
            AceQLConsole.WriteLine("Delete witht deleteOrderlogAll() done to clear all for test.");

            // Instantiate all elements of an Orderlog raw
            OrderlogRaw orderlogRaw = new OrderlogRaw();

            AceQLTransaction transaction = await connection.BeginTransactionAsync();

            try
            {
                // Insert a row
                int rows = await InsertRaw(orderlogRaw);
                AceQLConsole.WriteLine("Insert done. Rows: " + rows);
                Assert.True(rows == 1, "inserted rows must be 1");

                // Select same raw and make user all values get back are the same;
                await SelectRaw(orderlogRaw, false);
                await SelectRaw(orderlogRaw, true);
                AceQLConsole.WriteLine("Select done.");

                await transaction.CommitAsync();
            }
            catch (Exception exception)
            {
                AceQLConsole.WriteLine("Exception thrown: " + exception.ToString());
                await transaction.RollbackAsync();
            }

            transaction = await connection.BeginTransactionAsync();

            try
            {
                int rows = await UpdateRawQuantityAddOneThousand(orderlogRaw);
                AceQLConsole.WriteLine("Update done. Rows: " + rows);
                Assert.True(rows == 1, "updated rows must be 1");

                await SelectRawDisplayQuantity(orderlogRaw);
                AceQLConsole.WriteLine("Select quantity done.");

                await transaction.CommitAsync();
            }
            catch (Exception exception)
            {
                AceQLConsole.WriteLine("Exception thrown: " + exception.ToString());
                await transaction.RollbackAsync();
            }
        }

        private async Task SelectRawDisplayQuantity(OrderlogRaw orderlogRaw)
        {
            string sql = "select * from orderlog where customer_id = @customer_id and item_id = @item_id ";

            AceQLCommand command = new AceQLCommand(sql, connection);
            command.Parameters.AddWithValue("@customer_id", orderlogRaw.CustomerId);
            command.Parameters.AddWithValue("@item_id", orderlogRaw.ItemId);

            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                while (dataReader.Read())
                {
                    int quantity = dataReader.GetInt32(dataReader.GetOrdinal("quantity"));
                    Console.WriteLine("quantity: " + quantity);
                    Assert.True(quantity == orderlogRaw.Quantity + 1000, "quantity in select is not the same as updated");
                }
            }
        }

        private async Task<int> UpdateRawQuantityAddOneThousand(OrderlogRaw orderlogRaw)
        {
            String sql = "update orderlog set quantity = @parm1";
            AceQLCommand command = new AceQLCommand(sql, connection);
            command.Parameters.AddWithValue("@parm1", orderlogRaw.Quantity + 1000);

            int rows = await command.ExecuteNonQueryAsync();
            return rows;
        }

        private async Task<int> InsertRaw(OrderlogRaw orderlogRaw)
        {
            string sql =
            "insert into orderlog values (@parm1, @parm2, @parm3, @parm4, @parm5, @parm6, @parm7, @parm8, @parm9)";

            AceQLCommand command = new AceQLCommand(sql, connection);

            command.Parameters.AddWithValue("@parm1", orderlogRaw.CustomerId);
            command.Parameters.AddWithValue("@parm2", orderlogRaw.ItemId);
            command.Parameters.AddWithValue("@parm3", orderlogRaw.Description);
            //command.Parameters.Add(new AceQLParameter("@parm4", new AceQLNullValue(AceQLNullType.DECIMAL))); //null value for NULL SQL insert.
            command.Parameters.AddWithValue("@parm4", orderlogRaw.ItemCost);
            command.Parameters.AddWithValue("@parm5", orderlogRaw.DatePlaced);
            command.Parameters.AddWithValue("@parm6", orderlogRaw.DateShipped);

            Stream stream = new FileStream(orderlogRaw.JpegImage, FileMode.Open, FileAccess.Read);
            command.Parameters.Add(new AceQLParameter("@parm7", stream));

            command.Parameters.AddWithValue("@parm8", orderlogRaw.Delivered == true ? 1:0);
            command.Parameters.AddWithValue("@parm9", orderlogRaw.Quantity);

            int rows = await command.ExecuteNonQueryAsync();
            return rows;

        }

        private async Task SelectRaw(OrderlogRaw orderlogRaw, bool useColumnNames)
        {
            string sql = "select * from orderlog where customer_id = @customer_id and item_id = @item_id ";

            AceQLCommand command = new AceQLCommand(sql, connection);
            command.Parameters.AddWithValue("@customer_id", orderlogRaw.CustomerId);
            command.Parameters.AddWithValue("@item_id", orderlogRaw.ItemId);

            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                while (dataReader.Read())
                {
                    int customerId;
                    int itemId;
                    string description;
                    Double itemCost;
                    DateTime datePlaced;
                    DateTime dateShipped;
                    Stream stream;
                    bool isDelivered;
                    int quantity;

                    AceQLConsole.WriteLine();

                    if (!useColumnNames)
                    {
                        AceQLConsole.WriteLine("Get values using ordinal values:");
                        int i = 0;
                        customerId = dataReader.GetInt32(i++);
                        itemId = dataReader.GetInt32(i++);

                        description = dataReader.GetString(i++);
                        itemCost = dataReader.GetDouble(i++);
                        datePlaced = dataReader.GetDateTime(i++).Date;
                        dateShipped = dataReader.GetDateTime(i++);
                        stream = await dataReader.GetStreamAsync(i++);

                        isDelivered = dataReader.GetInt32(i++) == 1 ? true : false;
                        quantity = dataReader.GetInt32(i++);
                    }
                    else
                    {
                        AceQLConsole.WriteLine("Get values using column name values:");
                        int i = 0;
                        customerId = dataReader.GetInt32(dataReader.GetOrdinal("customer_id"));
                        itemId = dataReader.GetInt32(dataReader.GetOrdinal("item_id"));
                        description = dataReader.GetString(dataReader.GetOrdinal("description"));
                        itemCost = dataReader.GetDouble(dataReader.GetOrdinal("item_cost"));

                        datePlaced = dataReader.GetDateTime(dataReader.GetOrdinal("date_placed")).Date;
                        dateShipped = dataReader.GetDateTime(dataReader.GetOrdinal("date_shipped"));

                        stream = await dataReader.GetStreamAsync(dataReader.GetOrdinal("jpeg_image")); 

                        isDelivered = dataReader.GetInt32(dataReader.GetOrdinal("is_delivered")) == 1 ? true : false;
                        quantity = dataReader.GetInt32(dataReader.GetOrdinal("quantity"));
                    }

                    String jpegImageOut = AceQLTestParms.OUT_DIRECTORY + "\\username_koala.jpg";
                    try
                    {
                        
                        using (var fileStream = File.Create(jpegImageOut))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                    finally
                    {
                        stream.Close();
                    }

                    AceQLConsole.WriteLine("customer_id : " + customerId);
                    AceQLConsole.WriteLine("item_id     : " + itemId);
                    AceQLConsole.WriteLine("description : " + description);
                    AceQLConsole.WriteLine("item_cost   : " + itemCost);
                    AceQLConsole.WriteLine("date_placed : " + datePlaced.Date);
                    AceQLConsole.WriteLine("date_shipped: " + dateShipped);
                    AceQLConsole.WriteLine("jpeg_image  : " + orderlogRaw.JpegImage);
                    AceQLConsole.WriteLine("is_delivered: " + isDelivered);
                    AceQLConsole.WriteLine("quantity    : " + quantity);

                    Assert.True(customerId == orderlogRaw.CustomerId, "customer_id in select is not the same as insert");
                    Assert.True(itemId == orderlogRaw.ItemId, "item_id in select is not the same as insert");
                    Assert.True(description == orderlogRaw.Description, "description in select is not the same as insert");
                    Assert.True(itemCost == orderlogRaw.ItemCost, "item_cost in select is not the same as insert");

                    AceQLConsole.WriteLine("date_placed insert: " + orderlogRaw.DatePlaced);
                    AceQLConsole.WriteLine("date_placed select: " + datePlaced);

                    AceQLConsole.WriteLine("date_shipped insert: " + orderlogRaw.DateShipped);
                    AceQLConsole.WriteLine("date_shipped select: " + dateShipped);

                    Assert.True(datePlaced.Date == orderlogRaw.DatePlaced.Date, "date_placed in select is not the same as insert");
                    Assert.True(dateShipped.ToString().Equals(orderlogRaw.DateShipped.ToString()), "date_shipped in select is not the same as insert");

     
                    String sha1Insert = SHA1.GetChecksum(orderlogRaw.JpegImage);
                    String sha1Select = SHA1.GetChecksum(jpegImageOut);

                    AceQLConsole.WriteLine("jpeg_image SHA1 insert: " + sha1Insert + " (" + orderlogRaw.JpegImage + ")");
                    AceQLConsole.WriteLine("jpeg_image SHA1 select: " + sha1Select +" (" + jpegImageOut + ")");

                    Assert.True(isDelivered == orderlogRaw.Delivered, "is_delivered in select is not the same as insert");
                    Assert.True(quantity == orderlogRaw.Quantity, "quantity in select is not the same as insert");
                    
                    AceQLConsole.WriteLine();

                }
            }
        }
    }
}
