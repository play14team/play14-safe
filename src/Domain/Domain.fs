namespace Play14.Domain

open System

module ConstrainedType =

    /// Create a constrained string using the constructor provided
    /// Return Error if input is null, empty, or length > maxLen
    let createString fieldName ctor maxLen str = 
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s must not be null or empty" fieldName 
            Error msg
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i chars" fieldName maxLen 
            Error msg 
        else
            Ok (ctor str)

    /// Create a optional constrained string using the constructor provided
    /// Return None if input is null, empty. 
    /// Return error if length > maxLen
    /// Return Some if the input is valid
    let createStringOption fieldName ctor maxLen str = 
        if String.IsNullOrEmpty(str) then
            Ok None
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i chars" fieldName maxLen 
            Error msg 
        else
            Ok (ctor str |> Some)

    /// Create a constrained integer using the constructor provided
    /// Return Error if input is less than minVal or more than maxVal
    let createInt fieldName ctor minVal maxVal i = 
        if i < minVal then
            let msg = sprintf "%s: Must not be less than %i" fieldName minVal
            Error msg
        elif i > maxVal then
            let msg = sprintf "%s: Must not be greater than %i" fieldName maxVal
            Error msg
        else
            Ok (ctor i)

    /// Create a constrained decimal using the constructor provided
    /// Return Error if input is less than minVal or more than maxVal
    let createDecimal fieldName ctor minVal maxVal i = 
        if i < minVal then
            let msg = sprintf "%s: Must not be less than %M" fieldName minVal
            Error msg
        elif i > maxVal then
            let msg = sprintf "%s: Must not be greater than %M" fieldName maxVal
            Error msg
        else
            Ok (ctor i)

    /// Create a constrained string using the constructor provided
    /// Return Error if input is null. empty, or does not match the regex pattern
    let createLike fieldName  ctor pattern str = 
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s: Must not be null or empty" fieldName 
            Error msg
        elif System.Text.RegularExpressions.Regex.IsMatch(str,pattern) then
            Ok (ctor str)
        else
            let msg = sprintf "%s: '%s' must match the pattern '%s'" fieldName str pattern
            Error msg 

module Common =

    type String50 = private String50 of string
    type EmailAddress = private EmailAddress of string

    module String50 =

        let value (String50 str) = str

        let create fieldName str = 
            ConstrainedType.createString fieldName String50 50 str

        let createOption fieldName str = 
            ConstrainedType.createStringOption fieldName String50 50 str

    module EmailAddress =

        let value (EmailAddress str) = str

        let create fieldName str = 
            let pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
            ConstrainedType.createLike fieldName EmailAddress pattern str


module Contacts =

    type Twitter = private Twitter of Uri

    module Twitter =
        let create ( handle : string) : Twitter =
            let name = handle.Split('/', '@') |> Seq.last
            sprintf "https://twitter.com/%s" name |> Uri |> Twitter

    type Facebook = private Facebook of Uri

    module Facebook =
        let create ( page : string ) : Facebook =
            let name = page.Split('/', '@') |> Seq.last
            sprintf "http://facebook.com/%s" name |> Uri |> Facebook

    type LinkedIn = private LinkedIn of Uri

    module LinkedIn =
        let create ( account : string ) : LinkedIn =
            let name = account.Split('/', '@') |> Seq.last
            sprintf "https://www.linkedin.com/in/%s" name |> Uri |> LinkedIn

    type SocialMedia =
    | Twitter of Twitter
    | Facebook of Facebook
    | LinkedIn of LinkedIn

    type EmailAddress = private EmailAddress of string

    module EmailAddress = 
        let create ( email : string ) : EmailAddress =
            let regex = Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
            if not (email |> regex.IsMatch) then
                raise <| ArgumentException("email", "Email is invalid")

            email |> EmailAddress

    type Contact =
    | Email of EmailAddress
    | Social of SocialMedia

    module Contact =
        let createEmail (email : string) : Contact =
            email |> EmailAddress.create |> Email

        let createTwitter (name : string) : Contact =
            name |> Twitter.create |> Twitter |> Social 
        
        let createFacebook (name : string) : Contact =
            name |> Facebook.create |> Facebook |> Social

        let createLinkedIn (name : string) : Contact =
            name |> LinkedIn.create |> LinkedIn |> Social


module Sponsors =

    open Contacts

    type SponsorName = SponsorName of string

    type WebsiteUri = private WebsiteUri of Uri
    module WebsiteUri = 
        let create (uri : string) : WebsiteUri =
            // TODO: website uri checks
            uri |> Uri |> WebsiteUri

    type LogoUri = LogoUri of Uri
    module LogoUri = 
        let create (uri : string) : LogoUri =
            // TODO: logo uri checks
            uri |> Uri |> LogoUri

    type Sponsor = {
        Name : SponsorName
        Website : WebsiteUri
        Logo : LogoUri
        Contacts : Contact seq
    }

module Players =

    open System
    open Contacts

    type FirstName = FirstName of string
    type LastName = LastName of string
    type PlayerName = {
        FirstName : FirstName
        LastName : LastName
    }

    type Bio = PlayerBio of string
    type Company = PlayerCompany of string
    type Avatar = Avatar of Uri
    type AttendedEvent = AttendedEvent of string

    type Player = {
        Name : PlayerName
        Bio : Bio
        Company : Company
        Avatar : Avatar
        Contacts : Contact seq
        Events : AttendedEvent seq
    }

    type Host = Host of Player
    type Mentor = Mentor of Host

    type Create = PlayerName -> Player
    type MakeHost = Player -> Host
    type MakeMentor = Host -> Mentor

module Events =

    open System
    open Players
    open Sponsors
    
    type Image = Uri
    type EventName = EventName of string
    type Schedule = EventSchedule
    type TimeTable = EventTimeTable
    
    type EventStatus =
        | Draft
        | Published
        | Candelled
        | Rescheduled
        | Ended
    

    type VenueName = VenueName of string
    type VenueArea = VenueArea of string
    type VenueAddress = VenueAddress of string
    type VenueWebsite = VenueWebsite of Uri
    type MapUri = MapUri of Uri
    type Venue = {
        Name : VenueName
        Area : VenueArea
        Address : VenueAddress
        Website : VenueWebsite
        Map : MapUri
    }

    type EventVenue =
        | ToBeDefined
        | Venue of Venue
    
    type LinkRegistration = LinkRegistration of Uri
    
    type EventbriteId = EventbriteId of string
    type EventbriteRegistration = {
        EventId : EventbriteId
    }
    
    type WeezeventId = WeezeventId of string
    type WeezeventCode = WeezeventCode of string
    type WeezeventRegistration = {
        EventId : WeezeventId
        Code : WeezeventCode
    }
    
    type Registration =
       | NotOpened
       | Link of LinkRegistration
       | Eventbrite of EventbriteRegistration
       | Weezevent of WeezeventRegistration
       | Closed
    
    type Event = {
        Name : EventName
        Schedule : Schedule
        TimeTable : TimeTable
        Status : EventStatus
        Venue : EventVenue
        Images : Image seq
        TeamMembers : Host seq
        Mentors : Mentor seq
        Sponsors : Sponsor seq
    }

    type PublishedEvent = PublishedEvent of Event * Registration

    type UnpubishedEvent = UnpubishedEvent of Event
    type CancelledEvent = CancelledEvent of PublishedEvent
    type PostponedEvent = PostponedEvent of PublishedEvent
    type EndedEvent = EndedEvent of PublishedEvent

    type Create = EventName -> UnpubishedEvent
    type Publish = UnpubishedEvent -> Registration -> PublishedEvent
    type Cancel = PublishedEvent -> CancelledEvent
    type Postpone = PublishedEvent -> PostponedEvent
    type Reschedule = PostponedEvent -> PublishedEvent
    type MarkEnded = PublishedEvent -> EndedEvent
