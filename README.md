# BudgetApp (v.0.1)
A webpage for storing and displaying transaction data.

## Rationale

Warning! This is the non-technical part. You can skip this and go straight to the Configuration part unless you want to know why I made this application...

There are three major reasons behind the ignition of this project.

Necessity. As a student, trying to make ends meet, I needed to get control of my income and expenses. My solution back then was to add transaction data from my internet bank into google docs. This was an ok solution for a while, but the process of organizing transactions into categories on a monthly basis became too cumbersome in the long run. As a consequence, I promised myself that i would create a webpage to make my life alittle easier.

Curiosity. I knew that by creating this webpage I would get the oppertunity to try new technologies, e.g. Azure, Excelreader, authorization/authentication, GitHub and more.

Challenge. Working mostly as a front-end developer at my daily job, I now would have the oppertunity to try different roles.

## Configuration

This project is built ontop of the following tutorial: https://code.msdn.microsoft.com/MVC-5-with-2FA-email-8f26d952 which takes care of user accounts. It also uses a service to send emails (for verifying the user email and resetting passwords). To fully understand how this works i recommend to check out the tutorial.

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

### [SendGrid](http://sendgrid.com/)

I'm currently using a free plan through [Azure](http://azure.microsoft.com/) which includes 25 000 emails per month. However, SendGrid do have a free plan which includes 12 000 emails per month.

### External logins

#### [Google](https://console.developers.google.com/)

#### [Facebook](https://developers.facebook.com/)

#### [Twitter](https://apps.twitter.com/)
