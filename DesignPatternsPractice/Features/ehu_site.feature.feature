Feature: Verification of EHU website main features

  Scenario: Verification of contact information on the contacts page
    Given the user is on the contacts page
    When the user views the contact information
    Then the email "franciskscarynacr@gmail.com" is displayed
    And the LT phone "Phone (LT): +370 68 771365" is displayed
    And the BY phone "Phone (BY): +375 29 5781488" is displayed
    And the following social links are present: Facebook, Telegram, VK

  Scenario: Verification of main website navigation
    Given the user is on the home page
    When the user clicks the About link in the navigation
    Then the URL contains "/about/"