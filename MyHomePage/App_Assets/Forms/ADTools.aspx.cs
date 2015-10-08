using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class App_Assets_Forms_ADTools : System.Web.UI.Page
{
    private static string[] AD_Computers;

    protected void Page_Load(object sender, EventArgs e)
    {
        UserPanel.Visible = false;
        ComputerPanel.Visible = false;
        PrincipalContext pc = new PrincipalContext( ContextType.Domain, "ITSERVICES" );
        ComputerPrincipal computer = new ComputerPrincipal( pc );
        computer.Name = "*"; //reg expression
        PrincipalSearcher ps = new PrincipalSearcher();
        ps.QueryFilter = computer;
        ((System.DirectoryServices.DirectorySearcher)ps.GetUnderlyingSearcher()).PageSize = 500;
        PrincipalSearchResult<Principal> psr = ps.FindAll();
        AD_Computers = new string[psr.Count()];
        int i = 0;
        foreach ( ComputerPrincipal cp in psr )
        {
            AD_Computers[i++] = cp.Name;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        return (from c in AD_Computers where c.StartsWith( prefixText, StringComparison.CurrentCultureIgnoreCase ) select c).Take( count ).ToArray();
    }

    #region User

    protected void btnUser_Click(object sender, EventArgs e)
    {
        ChoicePanel.Visible = false;
        UserPanel.Visible = true;
    }

    #endregion User

    #region Computer

    protected void btnComputer_Click(object sender, EventArgs e)
    {
        ChoicePanel.Visible = false;
        ComputerPanel.Visible = true;
    }

    protected void fillBtn_Click(object sender, EventArgs e)
    {
        if ( isNull() )
            return;

        using ( PrincipalContext context = new PrincipalContext( ContextType.Domain, "ITSERVICES" ) )
        {
            ComputerPrincipal computer = new ComputerPrincipal( context );
            computer.Name = ComputerTextBox.Text;
            using ( PrincipalSearcher searcher = new PrincipalSearcher( computer ) )
            {
                Principal result = searcher.FindOne();

                AuthenticablePrincipal auth = result as AuthenticablePrincipal;
                if ( auth != null )
                {
                    lblName.Text = auth.Name;
                    lblLastLogon.Text = auth.LastLogon.ToString();
                    lblEnabled.Text = auth.Enabled.ToString();
                    lblDistinguishedName.Text = auth.DistinguishedName;
                }
            }
        }
        ComputerPanel.Visible = true;
    }

    protected void DeleteComputerBtn_Click(object sender, EventArgs e)
    {
    }

    #endregion Computer

    private bool isNull()
    {
        if ( String.IsNullOrWhiteSpace( ComputerTextBox.Text ) )
        {
            ComputerTextBox.BorderColor = System.Drawing.Color.Red;
            return true;
        }

        return false;
    }
}