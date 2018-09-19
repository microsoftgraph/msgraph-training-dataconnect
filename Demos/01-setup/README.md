# Setup Office 365 Tenant and Enable Euclid

Prior to leveraging Euclid for the first time, you need to configure your Office 365 tenant. This involves turning on the service and configuring a security group with permissions to approve data extraction requests. Users that will go in this group that will grant consent to data requests must have the **global administrator** role enabled.

## Prerequisites

To complete this lab, you need the following:

- Microsoft Azure subscription
  - If you do not have one, you can obtain one (for free) here: [https://azure.microsoft.com/free](https://azure.microsoft.com/free/)
  - The account used to signin must have the **global administrator** role granted to it.
- Office 365 tenancy
  - If you do not have one, you obtain one (for free) by signing up to the [Office 365 Developer Program](https://developer.microsoft.com/office/dev-program).
  - Multiple Office 365 users with emails sent & received
  - Access to at least two accounts that meet the following requirements:
    - global tenant administrators & have the **global administrator** role granted
    - have MFA enabled on both accounts

## Grant Azure AD users the **global administrator** role and enable MFA

In this step you will ensure that two users in your Office 365 tenant have the **global administrator** role enabled and enable multi-factor authentication for one of them.

1. Open a browser and navigate to your Azure Portal at [https://portal.azure.com](https://portal.azure.com)
1. Login using an account with global administrator rights to your Azure and Office 365 tenants.
1. Select **Azure Active Directory** (Azure AD) from the sidebar navigation or using the search bar:

    ![Screenshot selecting Azure AD](./../../Images/aad-user-setup-01.png)

1. On the Azure AD Overview page, select **Users** from the **Manage** section of the menu:

    ![Screenshot of the Users menu in Azure AD](./../../Images/aad-user-setup-02.png)

1. In the list of **All Users**, identify a user you will use in this lab that you have access to.
    1. Select the user by selecting their name.
    1. In the sidebar navigation menu, select **Directory role**.

        ![Screenshot of the Users menu in Azure AD](./../../Images/aad-user-setup-03.png)

    1. If the role **Global administrator** is not in the list of roles for the user:
        1. Select **Add role** button.
        1. Locate and select the **Global administrator** role and then select the **Select** button.
    1. Repeat these steps with another user that you will use in this lab.
1. On the **Users - All Users** page, select the **Multi-Factor Authentication** button at the top of the list of users.
1. Locate one of the two users that you granted the **Global administrator** role to. Select that user.

    > This is the user that will be used to approve the Euclid data requests you will initiate later in this lab. The user approving these requests must have multi-factor authentication (MFA) enabled.

    1. In the sidebar, select the **Enable** link under the list of **Quick Steps**.

        ![Screenshot of enabling MFA for users](./../../Images/aad-user-setup-04.png)

    1. Follow the prompts to enable multi-factor authentication on this user.

## Configure Euclid consent request approver group

In this step you will setup your Office 365 tenant to enable usage of Euclid.

1. Open a browser and navigate to your Microsoft 365 Admin Portal at [https://admin.microsoft.com](https://admin.microsoft.com)
1. In the sidebar navigation, select **Groups**.
1. Select the **Add a group** button.
1. Use the following to create the new mail-enabled security group and select the **Add** button.

    - **Type**: Mail-enabled security
    - **Name**: Consent Request Approvers

    ![Screenshot of creating a new mail-enabled security group](./../../Images/m365-group-setup-01.png)

1. Once the group has been created, select it.
1. On the **Members** section of the group dialog, select **Edit**
1. Add the two users that you enabled the **Global administrator** role to this new group.

## Enable Euclid in your Office 365 tenant

In this step you will enable the Euclid service on your Office 365 tenant.

1. While you are still logged into the Microsoft 365 Admin Portal, select the **Settings > Services & Add-ins** menu item.
1. Select the **Managed access to Microsoft Graph in Microsoft Azure Preview** service.

    ![Screenshot of the Managed access to Microsoft Graph in Microsoft Azure Preview settings](./../../Images/m365-setup-01.png)

1. Enable the toggle button at the top of the dialog to **Turn Managed access to Microsoft Graph in Microsoft Azure Preview on or off for your entire organization.**
1. Enter **Consent Request Approvers** (*or the name of the group you created previously*) in the **Group of users to make approval decisions** and select **Save**.
