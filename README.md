# Archaic Retail Manager

Walk along with Tim Corey's TimCo Retail Manager Course Introduction course on Youtube.

Playlist: https://www.youtube.com/playlist?list=PLLWMQd6PeGY0bEMxObA6dtYXuJOGfxSPx
Current Video: https://www.youtube.com/watch?v=k6mbESq--eE&list=PLLWMQd6PeGY0bEMxObA6dtYXuJOGfxSPx&index=22


## Note

* remember to add `bearer` before the auth token in Swagger

## Purpose

We work for Archaic Enterprise Solutions. We build solutions and sell them to clients. Our current project is to build a retail manager.

### Initial Project

* Requirements
  * build a desktop app that
    * cash register
    * handles inventory
    * manages the entire store
  * design for growth
    * web API layer that can service desktop, web, or mobile

* Plan
  * build an MVP - min viable product
    * get the major pieces in place
      * source control - Git on AzureDevOps
      * SQL database (SQL Server Data Tools) - not tied only to SQL
      * WebAPI with authentication
      * WPF application that can hit the API
      * desktop app (WPF) that can log in through the API

* Planned Technologies
  * Unit Testing
  * Dependency Injection - SOLID - dependecy inversion (Autofac)
  * WPF (Calibre and Micro)
  * ASP.NET MVC (Web Frontend)
  * .NET Core (3.0 so it works with our WPF desktop app)
  * SSTD
  * Git
  * Azure DevOps
  * Design Patterns
  * Async (keep app performant)
  * Reporting
  * WebAPI
  * Logging
  * Data Validation (Fluent validation)
  * HTML
  * CSS
  * Javascript
  * Authentication

## Tools Needed

* Postman - tool for simplify API dev
* Swagger

## Links Provided

### Swagger with WebAPI 2.0 login token

https://stackoverflow.com/questions/51117655/how-to-use-swagger-in-asp-net-webapi-2-0-with-token-based-authentication
