# BudgetApp
Experimental Project (ASP.NET MVC 5)

This project is built ontop of the following tutorial: https://code.msdn.microsoft.com/MVC-5-with-2FA-email-8f26d952

## Configuration

This project uses "external logins" (Facebook, Twitter and Google).
It also uses a service to send emails (for verifying the user email and resetting passwords).

In the Web.Config file you need to fill in values for these keys:

      <appSettings>
        <!-- SendGrid --> 
        <add key="mailAccount" value="mailaccounthere" />
        <add key="mailPassword" value="passwordhere" />
        
        <!-- GoogleAuth -->
        <add key="GoogClientID" value="xxxx-xxxxx.apps.googleusercontent.com" />
        <add key="GoogClientSecret" value="secrethere" />
        
        <!-- FacebookAuth -->
        <add key="appId" value="idhere" />
        <add key="appSecret" value="secrethere" />
        
        <!-- TwitterAuth -->
        <add key="consumerKey" value="keyhere" />
        <add key="consumerSecret" value="secrethere" />
        
        ...
        
       </appSettings>

### SendGrid (http://sendgrid.com/)

I'm currently using the free plan which includes 12 000 emails per month.

### Google (https://console.developers.google.com/)

### Facebook (https://developers.facebook.com/)

### Twitter (https://apps.twitter.com/)
