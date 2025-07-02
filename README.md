# unbuild

**unbuild** is a security-focused PoC tool designed to test whether a .NET project is vulnerable to build-time injection - a common vector in software supply chain attacks.

---

## Why It Matters

Build systems are an increasingly common target in real-world attacks. If malicious code can be injected into a build script or project file, it may:

- Exfiltrate secrets (like env vars or credentials)
- Execute payloads during build or publish
- Poison artifacts before they reach production

This tool **does not exfiltrate anything** — it simply tries to detect a broken build pipeline.

---

##  What It Does

- Injects a harmless `AfterBuild` target into your `.csproj` file
- Builds the project using `dotnet build`
- Detects whether injected commands are executed
- Cleans up the project afterward
- Alerts if the build process allows arbitrary code execution

> The goal: to simulate and detect insecure build environments, not to exploit them.

---
## Installation

Clone the repo and build:

```bash
git clone https://github.com/vrushalned/unbuild.git
cd unbuild
dotnet build
```
---

##  Usage

```bash
dotnet run --path "path/to/your/project.csproj"
```
## Sample Output

```bash
(un)building C:\Projects\MyApp\MyApp.csproj ...
Adding a fake afterbuild target to C:\Projects\MyApp\MyApp.csproj ...
Build C:\Projects\MyApp\MyApp.csproj ...
!!!POTENTIAL VULNERABILITY!!!
Your project has been (un)built! You need to up your game!
Clean up started!
Clean up complete!
```

## Ethical Usage Advisory

**unbuild** is intended **solely for authorized security testing** on projects and environments you own or have explicit permission to analyze.  
Unauthorized use against third-party systems, services, or software is strictly prohibited and may violate applicable laws and regulations.

By using **unbuild**, you agree to act responsibly and ethically. The authors disclaim any liability for misuse or damages arising from improper use.

Always obtain proper consent before testing any software or infrastructure.

