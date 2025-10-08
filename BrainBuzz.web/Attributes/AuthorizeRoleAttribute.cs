using Microsoft.AspNetCore.Components;
using BrainBuzz.web.Services.Interface;
using BrainBuzz.web.Constants;

namespace BrainBuzz.web.Attributes
{
    /// <summary>
    /// Attribute-based role authorization for Blazor components
    /// </summary>
    public class AuthorizeRoleAttribute : Attribute
    {
        public string[] Roles { get; }

        public AuthorizeRoleAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }
    
    /// <summary>
    /// Base component with role-based authorization
    /// </summary>
    public class AuthorizedComponentBase : ComponentBase
    {
        [Inject]
        protected IAuthenticationService AuthService { get; set; } = default!;
        
        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;
        
        protected bool IsAuthorized { get; set; } = false;
        protected bool IsCheckingAuth { get; set; } = true;
        protected string[] RequiredRoles { get; set; } = Array.Empty<string>();
        
        protected override async Task OnInitializedAsync()
        {
            await CheckAuthorizationAsync();
        }
        
        protected async Task CheckAuthorizationAsync()
        {
            IsCheckingAuth = true;
            
            var authResult = await AuthService.CheckAuthenticationAsync();
            
            if (!authResult.IsAuthenticated)
            {
                Navigation.NavigateTo("/login", forceLoad: true);
                return;
            }
            
            // If no specific roles required, just check authentication
            if (RequiredRoles == null || RequiredRoles.Length == 0)
            {
                IsAuthorized = true;
                IsCheckingAuth = false;
                return;
            }
            
            // Check if user has any of the required roles
            IsAuthorized = authResult.Roles.Any(userRole => RequiredRoles.Contains(userRole));
            
            if (!IsAuthorized)
            {
                // User is authenticated but doesn't have required role
                Navigation.NavigateTo("/unauthorized", forceLoad: true);
            }
            
            IsCheckingAuth = false;
        }
    }
}

