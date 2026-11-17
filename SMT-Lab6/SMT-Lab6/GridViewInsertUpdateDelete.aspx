
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="GridViewInsertUpdateDelete.aspx.cs"
    Inherits="SMT_Lab6.GridViewInsertUpdateDelete" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>GridView Insert Update Delete</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 25px;
            background-color: #fffafd;
        }

        h1 {
            color: #FF0066;
            text-align: center;
        }

        .form-table {
            width: 100%;
            max-width: 1000px;
            margin: auto;
        }

        .field-label {
            width: 150px;
            color: #FF0066;
            padding: 8px;
        }

        .input-field {
            width: 310px;
            max-width: 90%;
            padding: 7px;
            border: 1px solid #e7a1c0;
            border-radius: 4px;
            box-sizing: border-box;
        }

        .save-button,
        .cancel-button {
            width: 150px;
            max-width: 90%;
            padding: 12px;
            margin: 8px;
            cursor: pointer;
            border-radius: 4px;
        }

        .save-button {
            color: white;
            background-color: #a00050;
            border: none;
        }

        .cancel-button {
            color: white;
            background-color: #cc0000;
            border: none;
        }

        .save-button:hover {
            background-color: #7d003e;
        }

        .cancel-button:hover {
            background-color: #990000;
        }

        .message {
            color: #FF3399;
            font-weight: bold;
        }

        .grid-container {
            overflow-x: auto;
            margin-top: 25px;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
        }

        .grid th,
        .grid td {
            padding: 8px;
        }

        @media (max-width: 600px) {
            body {
                margin: 10px;
            }

            .field-label {
                width: 100px;
            }

            .input-field {
                width: 100%;
            }

            .save-button,
            .cancel-button {
                width: 120px;
                margin: 5px 0;
            }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <h1>GridView Insert, Update and Delete</h1>

        <!-- Input Form -->
        <table class="form-table">
            <tr>
                <td class="field-label">Name</td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"
                        CssClass="input-field"
                        MaxLength="50" />
                </td>
            </tr>

            <tr>
                <td class="field-label">Address</td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server"
                        CssClass="input-field"
                        MaxLength="50" />
                </td>
            </tr>

            <tr>
                <td class="field-label">Mobile No</td>
                <td>
                    <asp:TextBox ID="TextBox3" runat="server"
                        CssClass="input-field"
                        MaxLength="50" />
                </td>
            </tr>

            <tr>
                <td class="field-label">Email</td>
                <td>
                    <asp:TextBox ID="TextBox4" runat="server"
                        CssClass="input-field"
                        MaxLength="50" />
                </td>
            </tr>

            <tr>
                <td class="field-label">Roll No</td>
                <td>
                    <asp:TextBox ID="TextBox5" runat="server"
                        CssClass="input-field"
                        MaxLength="50" />
                </td>
            </tr>

            <tr>
                <td></td>
                <td>
                    <asp:Button ID="Button1" runat="server"
                        Text="Save"
                        CssClass="save-button"
                        OnClick="Button1_Click" />

                    <asp:Button ID="Button3" runat="server"
                        Text="Cancel"
                        CssClass="cancel-button"
                        CausesValidation="false"
                        OnClick="Button3_Click" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server"
                        CssClass="message"
                        EnableViewState="false" />
                </td>
            </tr>
        </table>

        <!-- Records Grid -->
        <div class="grid-container">

            <asp:GridView ID="GridView1" runat="server"
                CssClass="grid"
                AutoGenerateColumns="False"
                DataKeyNames="Id"
                AllowPaging="True"
                PageSize="10"
                OnPageIndexChanging="GridView1_PageIndexChanging"
                OnRowEditing="GridView1_RowEditing"
                OnRowCancelingEdit="GridView1_RowCancelingEdit"
                OnRowUpdating="GridView1_RowUpdating"
                OnRowDeleting="GridView1_RowDeleting"
                CellPadding="6"
                GridLines="Both"
                BackColor="White"
                BorderColor="#CC9966"
                BorderWidth="1px"
                EmptyDataText="No records found.">

                <Columns>

                    <asp:CommandField
                        ShowEditButton="True"
                        ShowDeleteButton="True"
                        EditText="Edit"
                        DeleteText="Delete"
                        CancelText="Cancel"
                        UpdateText="Update" />

                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <%# (GridView1.PageIndex * GridView1.PageSize)
                                + Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField
                        DataField="Name"
                        HeaderText="Name" />

                    <asp:BoundField
                        DataField="Address"
                        HeaderText="Address" />

                    <asp:BoundField
                        DataField="MobileNo"
                        HeaderText="Mobile No" />

                    <asp:BoundField
                        DataField="Email"
                        HeaderText="Email" />

                    <asp:BoundField
                        DataField="RollNo"
                        HeaderText="Roll No"
                        ReadOnly="True" />

                </Columns>

                <HeaderStyle
                    BackColor="#990000"
                    Font-Bold="True"
                    ForeColor="#FFFFCC" />

                <RowStyle
                    BackColor="White"
                    ForeColor="#330099" />

                <AlternatingRowStyle
                    BackColor="#FFF0F5" />

                <EditRowStyle
                    BackColor="#FFE0EC" />

                <PagerStyle
                    BackColor="#FFFFCC"
                    ForeColor="#330099"
                    HorizontalAlign="Center" />

            </asp:GridView>

        </div>

    </form>
</body>
</html>