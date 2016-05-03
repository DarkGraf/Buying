<%@ Application Language="C#" ClassName="Global" %>

<script runat="server">

    private static string address;
    public static string Address 
    { 
        get
        {
            if (address == null)
            {
                Uri uri = System.Web.HttpContext.Current.Request.Url;
                address = string.Format("{0}{1}{2}:{3}/Service.svc", uri.Scheme, Uri.SchemeDelimiter, uri.Host, uri.Port);        
            }
            return address;
        }
    }
    
    void Application_Start(object sender, EventArgs e) 
    {
        // Код, выполняемый при запуске приложения

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Код, выполняемый при завершении работы приложения

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Код, выполняемый при возникновении необрабатываемой ошибки

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Код, выполняемый при запуске нового сеанса
        
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Код, выполняемый при запуске приложения. 
        // Примечание: Событие Session_End вызывается только в том случае, если для режима sessionstate
        // задано значение InProc в файле Web.config. Если для режима сеанса задано значение StateServer 
        // или SQLServer, событие не порождается.

    }
       
</script>
