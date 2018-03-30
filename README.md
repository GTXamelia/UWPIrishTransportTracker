# UWP Irish Transport Tracker

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

<a href="https://imgur.com/a/cpQva"><img src="https://imgur.com/a/cpQva.gif" title="Luas Fail Test"/></a>

### And coding style tests

Explain what these tests test and why


## Deployment

Add additional notes about how to deploy this on a live system

## Built With

* [Dropwizard](http://www.dropwizard.io/1.0.2/docs/) - The web framework used
* [Maven](https://maven.apache.org/) - Dependency Management
* [ROME](https://rometools.github.io/rome/) - Used to generate RSS Feeds

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 

## Authors

* **Billie Thompson** - *Initial work* - [PurpleBooth](https://github.com/PurpleBooth)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Hat tip to anyone who's code was used
* Inspiration
* etc
