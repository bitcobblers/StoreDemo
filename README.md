# Storefront API Demonstration

This is a simple application that demonstrates basic functionality that a storefront might have (e.g., logging in, managing shopping carts, and purchasing things).

The purpose of this application is to demonstrate how `DrillSergeant` can be used to write behavior tests against a functioning API.

## Warning!  Achtung! 

This application is strictly meant for demonstration purposes.  It makes a lot of assumptions, hard-codes many things, and is in no way a suitable base to build anything that should ever be released in the wild.  Authentication is supported by using JWT (in a very insecure way) and it uses EntityFramework with SQLite to store content.  However the domain model only contains the bare amount needed to support the basic API calls provided by the controllers.  They are not optimized or indexed in any way.
