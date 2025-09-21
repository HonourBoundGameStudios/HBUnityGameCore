# UnityGameCore
This library was made MIT to assist small game studio to have a starting point.

Core engine for unity games at Honour Bound Game Studios Inc.


# Folder Structure

Many official Unity packages also implement this structure.

|Location|Description|
| :---: | --- |
|package.json	|The package manifest| , which defines the package dependencies and other metadata.|
|README.md	|Developer package documentation. This is generally documentation to help developers who want to change the package or push a new change on the package’s main branch.|
|CHANGELOG.md	|Description of package changes in reverse chronological order. It’s good practice to use a standard format, like Keep a Changelog.|
|LICENSE.md	|Contains the package license text. Usually the Package Manager copies the text from the selected SPDX list website.|
|Third Party Notices.md	|Contains information that’s required to meet legal requirements.|
|Editor/	|Editor platform-specific Assets folder. Unlike Editor folders under Assets, this is only a convention and doesn’t affect the Asset import pipeline. Refer to Assembly definition and packages to properly configure Editor-specific assemblies in this folder.|
|Runtime/	|Runtime platform-specific Assets folder. This is only a convention and doesn’t affect the Asset import pipeline. Refer to Assembly definition and packages to properly configure runtime assemblies in this folder.|
|Tests/	|Folder to store any tests included in the package.|
|Tests/Editor/	|Editor platform specific tests folder. Refer to Assembly definition and packages to properly configure Editor-specific test assemblies in this folder.|
|Tests/Runtime/	|Runtime platform specific tests. Refer to Assembly definition and packages to properly configure runtime test assemblies in this folder.|
|Samples~/	|Folder to store any samples included in the package.|
|Documentation~	|Folder to store any documentation included in the package.|

