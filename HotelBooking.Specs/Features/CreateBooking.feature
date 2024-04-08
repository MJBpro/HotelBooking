Feature: Create booking

    Scenario: Booking a room with no date overlaps
        Given I have selected a room
        And no bookings exist for the dates 2024-04-10 to 2024-04-12
        When I create a booking for these dates 2024-04-10 to 2024-04-12
        Then the booking should be successful

    Scenario: Attempting to book a room with date overlaps
        Given I have selected a room
        And a booking exists for the dates 2024-04-10 to 2024-04-12
        When I attempt to create a booking for these dates 2024-04-10 to 2024-04-12
        Then the booking should be declined
        
        
    Scenario: Attempting to book with a invalid booking
        Given I have selected an invalid booking
        When I attempt to book a any room
        Then an error should be thrown for invalid booking data
        
    Scenario: Attempting to book a room with startdate after enddate
        Given I have selected a room
        When I attempt to create a booking for these dates 2024-04-10 to 2024-02-12
        Then an error should be thrown for invalid dates