
using Microsoft.Graph;
using System.Threading.Tasks;

namespace AspNetCoreAzureStorage
{
    public class GraphApiClientService
    {
        private readonly GraphServiceClient _graphServiceClient;
        public GraphApiClientService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<IUserAppRoleAssignmentsCollectionPage> GetUserAppRolesforApp()
        {
            // src: https://github.com/425show/b2c-appRoles
            // application roles via Graph API
            var myAppid = "5511b8d6-4652-4f2f-9643-59c61234e3c7";

            var user = await _graphServiceClient
                .Me
                .Request()
                .GetAsync()
                .ConfigureAwait(false);

            // GET /users/{id | userPrincipalName}/appRoleAssignments
            var appRoles = await _graphServiceClient.Users[user.Id]
              .AppRoleAssignments
              .Request()
              .Filter($"appId eq '{myAppid}'")
              .GetAsync()
              .ConfigureAwait(false);

            //var servicePrincipalSearch = await _graphClient
            //    .ServicePrincipals
            //    .Request()
            //    .Select(x => new { x.Id, x.AppRoles })
            //    .Filter($"appId eq '{req.ApplicationId}'").GetAsync();


            return appRoles;
        }
    }
}
