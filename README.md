# This toolbox is no longer maintained in this repository

This toolbox is moved to the Authentication toolbox repository on Bitbucket. Please use that repository instead of this one.



# Delegation Toolbox

This toolbox extracts a (user's) JWT token from a custom header and makes it available to the application code.
It is used in secanrio's where an application with it's own authentication system acts on behalf of another user.

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->

- [Installation](#installation)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Installation

To add the toolbox to a project, you add the package to the csproj-file :

```xml
  <ItemGroup>
    <PackageReference Include="Digipolis.Delegation" Version="1.1.3" />
  </ItemGroup>
```

In Visual Studio you can also use the NuGet Package Manager to do this.

ALWAYS check the latest version [here](https://github.com/digipolisantwerp/delegation_aspnetcore/blob/master/src/Digipolis.Delegation/Digipolis.Delegation.csproj) before adding the above line !

## Contributing

Pull requests are always welcome, however keep the following things in mind:

- New features (both breaking and non-breaking) should always be discussed with the [repo's owner](#support). If possible, please open an issue first to discuss what you would like to change.
- Fork this repo and issue your fix or new feature via a pull request.
- Please make sure to update tests as appropriate. Also check possible linting errors and update the CHANGELOG if applicable.

## Support

Erik Seynaeve (<erik.seynaeve@digipolis.be>)
