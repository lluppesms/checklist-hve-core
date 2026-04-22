---
title: "Checklist HVE Core"
description: "A real-time collaborative checklist app for RV owners — because someone has to know if the slides are in before you drive away."
---

## The App That Could Save Your Marriage (and Your Slide-Outs)

You pull into a beautiful campsite after six hours of driving. The sun is setting. The kids are restless. Your partner is already walking around the RV pointing at things. You both have different mental checklists. Neither of you agrees on whether the water line is connected. Someone is going to forget to retract the awning. Again.

**Enter the Shared Checklist App.**

> "We argue less now. Mostly because the app tells us who forgot to turn off the water heater."
> — *A fictional but very relatable RV couple*

---

## What Is This

A mobile-first, real-time collaborative checklist application built for RV owners who need to coordinate setup and teardown tasks without yelling across the campsite — or worse, driving away with the steps still deployed.

- Select from pre-built templates (Arrival, Departure, Interior, Exterior, Vehicle)
- Check off tasks in real-time — your partner's screen updates instantly via SignalR
- Never again discover the sewer cap is still attached as you accelerate onto the highway
- Works on phones because you will absolutely be standing outside in the rain when you use it

---

## The Backing-In Problem

*(A moment of silence for everyone who has watched a fellow camper try to back a 40-foot fifth wheel into site 14B while their spouse stands behind it making increasingly frantic hand signals that mean completely different things to each of them.)*

This app does not solve backing in. No app can. Some things are beyond technology.

What it *does* solve is the part before and after that — all the tasks that need to happen in a specific order, done by two people, without either of them having to hold a laminated card in the wind.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor Web App (.NET 9) — Interactive Server rendering |
| Backend | ASP.NET Core Web API — Minimal APIs |
| Real-time | SignalR — so you can watch your partner check things off from across the campsite |
| Database | Azure SQL Database via EF Core + DACPAC for schema deployment |
| Orchestration | .NET Aspire 9.5 — because wiring things together should be boring |
| Auth | Microsoft Entra ID (Azure AD) — because not everyone should be able to mess with your checklists |
| Hosting | Azure App Service |
| IaC | Bicep — modular, parameterized, golden-code-aligned |
| CI/CD | GitHub Actions + Azure DevOps Pipelines (both fully functional) |

---

## Project Structure

```text
checklist-hve-core/
├── src/
│   ├── CheckList.Api/          # ASP.NET Core Web API (Minimal APIs, SignalR hub)
│   ├── CheckList.Web/          # Blazor Web App (mobile-first UI)
│   ├── CheckList.AppHost/      # .NET Aspire orchestration host
│   ├── CheckList.ServiceDefaults/  # Shared OTel, health checks, resilience
│   ├── CheckList.Shared/       # DTOs shared between Api and Web
│   └── CheckList.Tests/        # MSTest unit tests
├── infra/
│   └── bicep/                  # Azure infrastructure as Bicep modules
│       ├── main.bicep          # Top-level template
│       ├── main.bicepparam     # Token-replacement parameter file
│       ├── resourcenames.bicep # Centralized resource naming
│       ├── data/               # Resource abbreviation lookup table
│       └── modules/            # Individual resource modules
├── Database/
│   ├── CheckList.Database.sqlproj  # DACPAC project (Microsoft.Build.Sql)
│   ├── Schema/Tables/          # DDL for all tables
│   └── SeedData/               # 148 seed actions across 7 real-world RV checklists
├── .github/
│   ├── actions/login-action/   # Reusable OIDC/client-secret login composite action
│   └── workflows/              # Numbered + template GitHub Actions workflows
├── .azdo/pipelines/            # Azure DevOps pipelines (vars, jobs, stages, roots)
└── old-source/                 # The prototype. Look, but don't touch.
```

---

## Pipelines and Deployment

Pipelines follow the numbered golden-code pattern from [lluppesms/dadabase.demo](https://github.com/lluppesms/dadabase.demo):

| # | Workflow | Purpose |
|---|---|---|
| 1 | `1-deploy-bicep` | Deploy Azure infrastructure only |
| 2.1 | `2.1-bicep-build-deploy-webapp` | Deploy infra + build + deploy web app |
| 4 | `4-build-deploy-dacpac` | Build and deploy the SQL DACPAC schema |
| 6 | `6-pr-scan-build` | PR CodeQL scan + build validation |

Both GitHub Actions (`.github/workflows/`) and Azure DevOps (`.azdo/pipelines/`) pipelines are fully implemented and maintained in parallel. Yes, both. No, we are not sorry.

---

## Getting Started Locally

```bash
# Clone and restore
git clone https://github.com/lluppesms/checklist-hve-core.git
cd checklist-hve-core
dotnet restore

# Run with Aspire (starts SQL, API, and Web together)
cd src/CheckList.AppHost
dotnet run
```

The Aspire dashboard will open automatically. The API will be at `https://localhost:{port}` and the Blazor app will launch in your browser. Your checklist will be ready. Your campsite setup, however, is still your problem.

---

## The Seed Data

Seven real-world RV checklist templates are pre-loaded, containing 148 actions across categories like:

- **Arrival Exterior** — Level the rig, deploy slides, connect utilities (in the right order, please)
- **Arrival Interior** — Turn on the water heater *before* you need hot water, not after
- **Departure Exterior** — Retract everything. Everything. Check again.
- **Departure Interior** — Secure the dishes. Yes, all of them.
- **Vehicle Checklist** — Hitch, brake controller, mirrors, the whole pre-flight

> Pro tip: The departure checklist includes "check for items left on picnic table." This item exists because someone, somewhere, left something important on a picnic table. We do not speak of what it was.

---

## Specifications

For the original design specifications and requirements, see [specifications.md](specifications.md).

---

## Contributing

If you have suggestions, found a bug, or want to add a checklist category for "things that fall out of the cargo bay on I-95" — pull requests are welcome.

Please make sure the build passes before submitting. The app should compile. The RV backing-in experience, unfortunately, cannot be guaranteed to compile under any conditions.

---

*Built with love, SignalR, and the hard-won wisdom of many campground arrivals.*
# Shared Checklist App

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

