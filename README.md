# Sitecore Serialization Checker Demo

This repository demonstrates a simple yet effective approach to displaying serialization warnings in Sitecore based on configured item paths and scopes defined within `.module.json` files.

## Overview

The **SerializedItemChecker** class provides functionality to:

* Load and parse `.module.json` files containing item serialization configurations.
* Evaluate Sitecore items against these configurations based on serialization scopes (`SingleItem`, `ItemAndChildren`, `ItemAndDescendants`).
* Display relevant warnings in the Sitecore Content Editor to indicate serialized items.

## Key Features

* **Automatic Parsing**: Reads and processes serialization configurations directly from `.module.json` files.
* **Flexible Scopes**: Supports multiple serialization scopes for precise control.
* **Easy Integration**: Simple to integrate into existing Sitecore pipelines and Helix-compliant solutions.

## Getting Started

### Prerequisites

* Sitecore XP
* .NET Framework 4.8
* Newtonsoft.Json

### Installation

1. Clone the repository:

```bash
git clone https://github.com/yourusername/sitecore-serialization-checker-demo.git
```

2. Ensure your `.module.json` files are located in:

```
~/App_Data/SerializationJsons
```

3. Include the project in your Sitecore solution and build the solution.

### Usage

The main class to use is `SerializedItemChecker`. Initialize it by calling:

```csharp
var checker = new SerializedItemChecker();
var (isSerialized, moduleFile) = checker.IsItemSerialized(sitecoreItem);

if (isSerialized)
{
    // Handle serialized item logic, e.g., display warnings.
}
```

### Example Configuration (`.module.json`)

```json
{
  "namespace": "Tenant",
  "items": {
    "includes": [
      {
        "name": "MySite",
        "path": "/sitecore/content/MyTenant/MySite",
        "scope": "SingleItem"
      },
      {
        "name": "Presentation",
        "path": "/sitecore/content/MyTenant/MySite/Presentation"
      }
    ]
  }
}
```

* If no `scope` is specified, the default `ItemAndDescendants` scope is assumed, as per SCS default configuration.

## Contributions

Contributions and feedback are welcome. Please create an issue or submit a pull request for any suggestions or improvements.

## License

This project is licensed under the MIT License.
