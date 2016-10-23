# KSyamkrishnan

# Problem Description
Part A: 
Create a console based application that accepts movie details like Name, Run Time, Language, Lead Actor and Genre and then export these details into one of the 2 formats, depending on user’s choice - “plain text” or “PDF”.

Part B: 
Make the application extensible so that it is easy to “plug in” a new format and the application automatically has the capability to export to the new format without making any changes to the application code.

# About the solution architecture
This is a simple console application written in c# that enables a user to add details about his/her watched movies ,display the details on the console and also to export data .

The details that a user is prompted to enter is configurable and can be configured using an xml file .The Xml file path ought to be given in app.config against the key "MovieDetailsFilePath" .

The above mentioned xml should be of the format :

```xml
<movie-details>
  <movie-detail type="string" display-text="Enter name of movie" mandatory="1" allow-multiple="0" persisted-text="Movie name">Name</movie-detail>
  <movie-detail type="time">Run Time</movie-detail>
  ..
  ..
</movie-details>
```

Each "movie-detail" node corresponds to a single entry the user would be prompted to enter. The attributes that I have defined and its behaviours are as follows:
display-text :This would be the text shown in console when the user is prompted to enter value for this movie detail.
mandatory: when values is 1,it means that the user cannot skip entering this attribute.
allow-multiple :The user can choose to have multiple entries for this movie detail.
persisted-text: This would be the text shown in pdf,text file or any other exported media.
type : This attribute dictates the behaviour of the movie detail. Currently 2 types are supported :string and time .
The application can be extended to support another type trivially by writing a class that inherits from "ISingleDetail" interface.
ISingleDetail interface has 2 methods :
Parse - takes an xmlnode as input and parses the specification attributes of the node. This method would be called when parsing the specification xml.
AddMovieDetail - This method decides the behaviour of the movie detail and returns an XElement that has the format :

```xml
<movie-name>qwe</movie-name>
```
where moive-name is the persisted-text or label that is required in the pdf or plain text.
In case of multiple entries  the expected format is :
```xml
<lead-actor multiple="1">
    <value>qwe</value>
    <value>swe</value>
 </lead-actor>
 ```
 
 This way movie details can be added or removed as required without altering the code and also new types can be supported and its attributes can be a completely distinct set.
 
# Registering a Detail Type.
 Registering Detail type handlers are driven dynamically at run time using an xml. The path of this xml ought to be specified in app.config appsettings section against the key **SingleDetailHandlerPath** . Each handler must derive from the interface **ISingleDetail** and must have a constructor that takes 0 arguments. 
 
# Exporting data
 
 Exporting data relies on the same desing pattern as above. The application exposes an interface **IExportMovie** that has a property displaytext and a method export that accepts a list of xelements (each xelement represents a movie) and the task of exporting is enthrusted on this export method of IExportMovie. The path of the handler registration xml ought to be specified in app.config appsettings section against the key **ExportHandlerPath**
 
 Each xelement would be of the form :
 ```xml
 <movie>
  <movie-name>qwe</movie-name>
..
..
</movie>
```

The application currently has 3 export handlers: for plain text , pdf and csv.

To define another export handler simply create an inherited class from IExportMovie interface and register itself as a handler in the exporthandler configuration xml file.







 
 
 
 
 
 



