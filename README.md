# KSyamkrishnan

Problem Description
Part A: 
Create a console based application that accepts movie details like Name, Run Time, Language, Lead Actor and Genre and then export these details into one of the 2 formats, depending on user’s choice - “plain text” or “PDF”.

Part B: 
Make the application extensible so that it is easy to “plug in” a new format and the application automatically has the capability to export to the new format without making any changes to the application code.

About the solution architecture
This is a simple console application written in c# that enables a user to add details about his/her watched movies ,display the details on the console and also to export data .

The details that a user is prompted to enter is configurable and can be configured using an xml file .The Xml file path ought to be given in app.config against the key "MovieDetailsFilePath" .

The above mentioned xml should be of the format :
"<movie-details>
  <movie-detail type="string" display-text="Enter name of movie" mandatory="1" allow-multiple="0" persisted-text="Movie name">Name</movie-detail>
  <movie-detail type="time">Run Time</movie-detail>
  ..
  ..
</movie-details>"

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
<movie-name>qwe</movie-name>
where moive-name is the persisted-text or label that is required in the pdf or plain text.
In case of multiple entries  the expected format is :
<lead-actor multiple="1">
    <value>qwe</value>
    <value>swe</value>
 </lead-actor>
 
 This way movie details can be added or removed as required without altering the code and also new types can be supported and its attributes can be a completely distinct set.
 
 Registering a Detail Type.
 The design is such that each implementation of ISingleDetail introduces itself to the application and states that it handles the requisite type.
 In code semantics this translates to a single function call "SingleDetail.RegisterDetailHandler("time", CreateTimeDetail);"
 where "time" is the type attribute value this class handles and CreateTimeDetail is a function that can create an instance of the class.
 Put this function call in a static function that takes 0 parameters and decorate it with the attribute [RegisterSingleDetailAttribute] and a new detail type is registerd in the application dynamically, all thanks to the magical prowess of reflection!
 
 Exporting data
 
 Exporting data relies on the same desing pattern as above. The application exposes an interface "IExportMovie" that has a property displaytext and a method export that accepts a list of xelements (each xelement represents a movie) and the task of exporting is enthrusted on this export method of IExportMovie.
 
 Each xelement would be of the form :
 <movie>
  <movie-name>qwe</movie-name>
..
..
</movie>

The application currently has 2 export handlers: for plain text and for pdf.

To define another export handler simple create an unherited class from IExportMovie interface and register itself as a handler to the application by defining a static method in the class decorated with the attribute "[RegisterExportMovie]"  and inside the function call the handler register function  RegisterExportMovieHandler .

When the user chooses to export data the application uses refection to register all handlers in the assembly pool and then populates the export choice options accordingly.

We can also reliably replace an existing handler  by making use of the third parameter to the handler registerfunction (pass boolean true as the third parameter).



 
 
 
 
 
 



