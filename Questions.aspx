<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Questions.aspx.cs" Inherits="iIECaB.Questions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
 
    
 
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Table style="width: 919px" ID="Table1" runat="server">
<asp:TableRow VerticalAlign="Middle" HorizontalAlign="Center">
    <asp:TableCell><asp:Label ID="LevelLabel"  Font-Size="1cm" Width="2cm" Height="2cm" runat="server"></asp:Label></asp:TableCell>
</asp:TableRow>
<asp:TableRow id="Row1">

   <asp:TableCell VerticalAlign="Bottom" HorizontalAlign="Center" id="Images">
    <asp:Image ID="Image1" runat="server" Height="4cm" Width="4cm" 
           ImageAlign="Middle"/>
    <asp:Image ID="Image2" runat="server" Height="4cm" Width="4cm" 
           ImageAlign="Middle" />
  </asp:TableCell>
  
   </asp:TableRow>
   <asp:TableRow VerticalAlign="Middle" HorizontalAlign="Center">
   <asp:TableCell>
   <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
   </asp:TableCell>
   </asp:TableRow>
   <asp:TableRow>
   <asp:TableCell>
   <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TextBox1" runat="server" Text="Please enter input" ErrorMessage="Please enter the answer and click Submit!!">
    </asp:RequiredFieldValidator><asp:Label ID="Label2" runat="server" Text="Label"/></asp:TableCell></asp:TableRow><asp:TableRow><asp:TableCell>
    <asp:Button ID="Button1" runat="server" CausesValidation="False" 
        onclick="Button1_Click" Text="Submit" />
        
    </asp:TableCell></asp:TableRow></asp:Table></asp:Content>