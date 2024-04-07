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