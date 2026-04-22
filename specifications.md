---
title: "Shared Checklist App — Specifications"
description: "Original design specifications and requirements for the Shared Checklist App."
---

## Shared Checklist App

The concept for this app started while setting up an RV when arriving in a campground and when preparing to leave the campground. There are a LOT of tasks to do that must be completed and must be done in a specific order.  There are usually two people working and they need to communicate what has been done and what is left to do before they are ready to drive off down the road.

Each time they enter a campground, there is a specific set of tasks that must be completed.  The list never changes (at least not much), but they need to make sure each task is done each time the enter a campground.  When they leave a campground, there is a set of tasks that must be completed.  The list never changes (at least not much), but they need to make sure each task is done each time they leave a campground.  There are multiple checklists of tasks, including interior and exterior tasks and tasks for the vehicle as well.

This app keeps several templated lists of tasks that the users can choose from. Once they select a list of tasks, it creates a copy of that list for this specific point in time so they can update those tasks as they complete them.  If one user completes a task, the other user should immediately see that the task is completed on their screen.  

Users will interact with it primarily on their phones, so this is primarily a mobile web site.  The users will be in a campground and they will have their phones out checking off tasks as they complete them.  They will be in a hurry to get on the road, so the app needs to be simple and easy to use.

## Tech Stack

The app should be coded in C# and Blazor, with an Aspire startup front-end.  The backend will be an ASP.NET Core Web API that will handle the database interactions and real-time updates using SignalR.  The database will be a simple database that will store the checklists and tasks. Preferably, the database will be hosted in Azure SQL Database. It could be another type of database if that works better, but there will be frequent updates an queries, so databases like Cosmos may not be well optimized for that.  The app will be hosted in Azure App Service.  The communications aspect should use SignalR to provide real-time updates to both users when one user completes a task.  The app should be responsive and work well on mobile devices.  The app should also be secure and protect user data.

## Reference

This project should have Azure DevOps pipelines set up for CI/CD and also GitHub Action workflows.  Both should be fully functional, and they should be well-documented with comments and a README file.  The Azure resources should be deployed via Bicep. Use the excellent https://github.com/lluppesms/dadabase.demo repository as a golden code example repo. The pipelines and Bicep files should be based on the ones in that repo, but they should be modified to fit the needs of this app.  The code should be well-structured and follow best practices for C# and Blazor development. 

## Proof of Concept Example

A prototype was done that implements much of this functionality, but it is not well-structured or complete and does not follow best practices.  The code is available in the `old` branch of this repository.  It can be used as a reference, but it should not be copied or used as a starting point for the new app.  The new app should be built from scratch, using the old code as a reference only when necessary.

The database structure is well designed and should be very close to what is needed as that structure seemed to fit the requirements well. Use that to create a SQL DACPAC project to deploy that application with the existing lists as sample data.

