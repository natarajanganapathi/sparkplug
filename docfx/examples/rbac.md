# RBAC Spec

# Models

Resources
1. Id
2. Resource
// 3. Verb
4. Scops

## Ex:
Id  scope   scope-group     resource
----------------------------------------------------------
1   read     user-mgt       get /user/{id}, 
2   write    user-mgt       post /user/{id}
3   read     user-mgt       get /user/{id}/profile
3   manage   user-mgt       get /user/{id}/profile

Id  scopes                  scope-group    resource
----------------------------------------------------------
1   read, write, manage     user-mgt       /user/{id} 





Scope
Read,
Query,
Write,
Message,



Users
1. Id
2. User

Groups
1. Id
2. Group

UserGroups
1. UserId
2. GroupId

Roles
1. Id
2. Name


RoleResourcePermissionMap


Permissions
1. Id




