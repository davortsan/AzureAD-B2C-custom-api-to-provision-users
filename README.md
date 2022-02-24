# AzureAD-B2C-custom-api-to-provision-users
.NET Core API to provision B2C user using Microsoft Graph API

First off, it would be necessary to create a new App registration in your Azure AD B2C portal. In my case, **GraphSKD** app was created with the following configuration:  
  
1. In **Authentication** check that _Access tokens (used for implicit flow)_ and _ID tokens (used for implicit and hybrid flows)_ are not checked  
  
![image](https://user-images.githubusercontent.com/2305432/155539808-66bf4828-fee1-40fc-b0ab-a89e53846383.png)
  
2. Create a new client secret that it will be used by the application AzureAD-B2C-custom-api-to-provision-users.    
**Note**  
Remember that you have to copy the value of the secret before to leave the Certificates & secrets page.  
  
3. Access to **API permissions** and add the following ppermissions to this application:  
  
![image](https://user-images.githubusercontent.com/2305432/155540608-10125402-54ab-4e94-8b68-8fb8416edb96.png)
  
**Note**  
Check that it is necessary to click on _Grant admin consent for <your_company>_. Review that all permissions are showed with a verified icon in the Status column.  

Ok, all is ready to open the code and type your specific app information. The information that you are going to need is:  
  
- ApplicationID  
- Client secret value  
- _b2c-extensions-app. Do not modify. Used by AADB2C for storing user data_ application id  

**Note**  
_b2c-extensions-app. Do not modify. Used by AADB2C for storing user data_ is a default application that is created automatically when your Azure AD B2C tenant is created. You can view this application if you access to App registrations and click on **All applications**
