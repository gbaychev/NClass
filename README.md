# NClass 

## Build status
| Branch | Status |
| -----  | -----  |
| Master | [![Build status](https://baychev.visualstudio.com/NClass/_apis/build/status/NClass-master)](https://baychev.visualstudio.com/NClass/_build/latest?definitionId=3) |
| Stable | [![Build status](https://baychev.visualstudio.com/NClass/_apis/build/status/NClass-stable)](https://baychev.visualstudio.com/NClass/_build/latest?definitionId=4) |

## About

NClass is a free tool to easily create UML class diagrams with C# and Java language support. The user interface is designed to be simple and user-friendly for easy and fast development. Initially the project was developed by the great Balasz Tihanyi on [sourceforge](http://nclass.sourceforge.net/), but the project has gone inactive due the lack of time. 

Still, NClass is a great tool, although it is missing some important features. The goal of this project is to restart the development of NClass, adding the missing features. 

## :computer: Existing features

 - C# and Java support with many language specific elements
 - Simple and easy to use user interface
 - Inline class editors with syntactic parsers for easy and fast editing
 - Source code generation
 - Reverse engineering from .NET assemblies (thanks to Malte Ried)
 - Configurable diagram styles
 - Printing / saving to image
 - Multilingual user interface
 - Mono support for non-Windows users

## :construction: Some of the things to come

  - Bringing the NClass' support of C# and Java up-to-date with current version of the languages
  - Sequence diagrams
  - Use Case diagrams -> done ✔️
  - Undo/Redo -> done ✔️
  - Many more to come. If you have an idea, don't hesitate to open an issue

## 🛠️ How to build
- at root folder level run:

        git submodule sync --recursive

- then go to < root >\src\AssemblyImport\lib\NReflect build nreflect with visual studio or run
        
        msbuild /p:Configuration=Debug /p:Platform="Any CPU"

  This implies that you have msbuild in your path.

- build NClass with Visual Studio or msbuild

:page_with_curl: License 
----

NClass is totally free and licensed under the [GNU General Public License](http://nclass.sourceforge.net/).
