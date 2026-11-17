
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SMT_Lab6
{
    public partial class GridViewInsertUpdateDelete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        // SELECT: Display all records
        private void BindGrid()
        {
            try
            {
                string query =
                    "SELECT Id, Name, Address, MobileNo, Email, RollNo " +
                    "FROM MyTable ORDER BY Id DESC";

                using (SqlConnection connection =
                    new SqlConnection(DBClass.connectionString))
                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    GridView1.DataSource = table;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Label1.Text = "Unable to load records: " +
                    Server.HtmlEncode(ex.Message);
            }
        }

        // INSERT: Save a new record
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox1.Text) ||
                string.IsNullOrWhiteSpace(TextBox5.Text))
            {
                Label1.Text = "Name and Roll No are required.";
                return;
            }

            string query =
                "INSERT INTO MyTable " +
                "(Name, Address, MobileNo, Email, RollNo) " +
                "VALUES (@Name, @Address, @MobileNo, @Email, @RollNo)";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(DBClass.connectionString))
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50)
                        .Value = TextBox1.Text.Trim();

                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 50)
                        .Value = TextBox2.Text.Trim();

                    command.Parameters.Add("@MobileNo", SqlDbType.NVarChar, 50)
                        .Value = TextBox3.Text.Trim();

                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 50)
                        .Value = TextBox4.Text.Trim();

                    command.Parameters.Add("@RollNo", SqlDbType.NVarChar, 50)
                        .Value = TextBox5.Text.Trim();

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                ClearFields();
                GridView1.EditIndex = -1;
                GridView1.PageIndex = 0;

                Label1.Text = "Record saved successfully!";
                BindGrid();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    Label1.Text =
                        "This Roll No already exists. Please use another one.";
                }
                else
                {
                    Label1.Text = "Database error: " +
                        Server.HtmlEncode(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Label1.Text = "Error: " +
                    Server.HtmlEncode(ex.Message);
            }
        }

        // CANCEL: Clear the input fields
        protected void Button3_Click(object sender, EventArgs e)
        {
            ClearFields();
            Label1.Text = "";
        }

        private void ClearFields()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
        }

        // PAGING: Change GridView page
        protected void GridView1_PageIndexChanging(
            object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.EditIndex = -1;
            BindGrid();
        }

        // EDIT: Enter edit mode
        protected void GridView1_RowEditing(
            object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            Label1.Text = "";
            BindGrid();
        }

        // CANCEL EDIT: Exit edit mode
        protected void GridView1_RowCancelingEdit(
            object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            Label1.Text = "";
            BindGrid();
        }

        // UPDATE: Save edited record
        protected void GridView1_RowUpdating(
            object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string name = Convert.ToString(e.NewValues["Name"]);
            string address = Convert.ToString(e.NewValues["Address"]);
            string mobileNo = Convert.ToString(e.NewValues["MobileNo"]);
            string email = Convert.ToString(e.NewValues["Email"]);

            if (string.IsNullOrWhiteSpace(name))
            {
                e.Cancel = true;
                Label1.Text = "Name cannot be empty.";
                return;
            }

            string query =
                "UPDATE MyTable SET Name=@Name, Address=@Address, " +
                "MobileNo=@MobileNo, Email=@Email WHERE Id=@Id";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(DBClass.connectionString))
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50)
                        .Value = name.Trim();

                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 50)
                        .Value = address ?? "";

                    command.Parameters.Add("@MobileNo", SqlDbType.NVarChar, 50)
                        .Value = mobileNo ?? "";

                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 50)
                        .Value = email ?? "";

                    command.Parameters.Add("@Id", SqlDbType.Int)
                        .Value = id;

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    GridView1.EditIndex = -1;

                    Label1.Text = rowsAffected > 0
                        ? "Record updated successfully!"
                        : "Record was not found.";

                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Label1.Text = "Update failed: " +
                    Server.HtmlEncode(ex.Message);
            }
        }

        // DELETE: Remove selected record
        protected void GridView1_RowDeleting(
            object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            string query = "DELETE FROM MyTable WHERE Id=@Id";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(DBClass.connectionString))
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int)
                        .Value = id;

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    Label1.Text = rowsAffected > 0
                        ? "Record deleted successfully!"
                        : "Record was not found.";
                }

                BindGrid();
            }
            catch (Exception ex)
            {
                Label1.Text = "Delete failed: " +
                    Server.HtmlEncode(ex.Message);
            }
        }
    }
}
