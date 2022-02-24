# AzureAD-B2C-custom-api-to-provision-users
.NET Core API to provision B2C user using Microsoft Graph API

First off, it would be necessary to create a new App registration in your Azure AD B2C portal. In my case, **GraphSKD** app was created with the following configuration:  
  
1. In **Authentication** check that _Access tokens (used for implicit flow)_ and _ID tokens (used for implicit and hybrid flows)_ are not checked  
  
![image](https://user-images.githubusercontent.com/2305432/155539808-66bf4828-fee1-40fc-b0ab-a89e53846383.png)
  

