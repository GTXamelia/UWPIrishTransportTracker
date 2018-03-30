<a href="https://imgur.com/jZR31CW"><img src="https://imgur.com/jZR31CW.png" title="Bus Adding Test"/></a>

A UWP application that allows the user to add any transport type from the TFI API.

## Getting Started

- Create blank visual studio UWP application
- Right click on project folder
- Git init
- Git remote add origin https://github.com/cian2009/UWPIrishTransportTracker.git
- Git pull origin master

### Prerequisites

Json parser for c#:
[Newtonsoft Json.NET](https://www.newtonsoft.com/json)

### Installing

A step by step series of examples that tell you have to get a development env running

Create Visual Studio 2017 Project

```
File > New > Project > Visual c# > Blank App (Universal Windows)
```

Set-up Git Repository

```
Git init
Git remote add origin https://github.com/cian2009/UWPIrishTransportTracker.git
Git pull origin master
```

## Running the tests

### Adding Transport

Adding a transport is the first phase of the program

1. Launch application
2. Click 'Add Transport'
3. Type in a valid stopID
	- Bus example: 	 522691
	- Train example: galwy
	- Luas example:  luas10

<a href="https://imgur.com/Uy8LRjL"><img src="https://imgur.com/Uy8LRjL.gif" title="Bus Adding Test"/></a>

### Adding Transport

Trying to view trasnport that hasn't been added

1. Launch application
2. Click 'View Luas'

<a href="https://imgur.com/LSu8tZ0"><img src="https://imgur.com/LSu8tZ0.gif" title="Luas Fail Test"/></a>

## Deployment

Add additional notes about how to deploy this on a live system

## Built With

* [Visual Studio](https://www.visualstudio.com/) - IDE used
* [Newtonsoft Json.NET](https://www.newtonsoft.com/json) - Json parser for c#
* [StackOverflow](https://stackoverflow.com/) - Community forum for troubleshooting

## Authors

* **Cian Gannon** - *All source code and UI* - [Github](https://github.com/cian2009)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE) file for details

## Acknowledgments

* StackOverflow - How did people code before you existed
