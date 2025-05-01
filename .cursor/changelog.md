# Changelog

## 2025-03-05

- Completed performance optimization tasks:
  - Added performance tests for key builder classes:
    - BlockBuilder
    - RichTextBuilder
    - SectionBuilder
    - SelectMenuBaseExtensions
  - Established performance baselines for all builder operations
  - Verified that all builder classes are performing efficiently (< 0.02ms per operation)
  - Updated project plan to mark performance optimization tasks as completed
- Performance test results:
  - RichTextBuilder: 0.007 ms per iteration
  - SelectMenuBaseExtensions: 0.013 ms per iteration
  - BlockBuilder and SectionBuilder also showed excellent performance

## 2023-03-09

- Completed unit tests for all remaining builder classes:
  - Added unit tests for ButtonExtensions
  - Added unit tests for CheckboxGroupExtensions
  - Added unit tests for OverflowMenuExtensions
  - Added unit tests for RadioButtonGroupExtensions
  - Fixed issues with OptionGroupBuilder to properly initialize Options collection
  - Fixed issues with SelectMenuBaseExtensions to properly initialize Label property
- Updated project plan to mark all unit test tasks as completed
- All 63 unit tests are now passing

## 2023-03-08

- Improved test coverage for the SlackNetBlockBuilder library:
  - Added unit tests for ContextBlockBuilder
  - Added unit tests for DateTimePickerExtensions
  - Added unit tests for InputBlockBuilder
  - Updated project plan to track test coverage progress
  - Organized test structure to match the library's architecture
- Identified remaining builder classes that need test coverage

## 2023-03-07

- Added comprehensive examples to the documentation:
  - Created modal dialog example showing how to build and handle modal dialogs
  - Created message updating example demonstrating various techniques for updating messages
  - Created interactive message handling example showing how to handle user interactions
- Updated project plan to mark completed example tasks
- Added new advanced examples and performance optimization tasks to the project plan
- Completed all planned documentation examples

## 2023-03-06

- Completed all documentation tasks in the initial project plan
- Marked all documentation tasks as completed in the project plan
- Added new next steps to the project plan for future development
- Prepared for NuGet package creation and CI/CD setup

## 2023-03-05

- Added XML documentation comments to all code files in the SlackNetBlockBuilder project
- Created detailed documentation structure in the docs directory:
  - Added main README.md for the docs directory
  - Added getting-started.md guide with installation and basic usage examples
  - Added API documentation for BlockBuilder and ButtonExtensions
  - Added interactive form example
  - Added advanced rich text formatting guide
- Updated project plan to reflect documentation progress
- Updated documentation to reflect that all builder classes are in the SlackNet.Blocks namespace (no additional using statement required)

## 2023-03-04

- Created project plan with initial tasks
- Created changelog to track project progress
- Identified all code files requiring XML documentation

## 2023-07-12
- Initiated documentation process
- Created project plan with initial tasks
- Started adding XML documentation comments to code files
- Completed XML documentation for ActionElementBuilder.cs, ActionsBlockBuilder.cs, and BlockBuilder.cs
- Completed XML documentation for BlockBuilderExtensions.cs, BlockElementBuilder.cs, ButtonExtensions.cs, and CheckboxGroupExtensions.cs
- Completed XML documentation for ContextBlockBuilder.cs, DateTimePickerExtensions.cs, InputBlockBuilder.cs, InputElementBuilder.cs, and OptionGroupBuilder.cs
- Completed XML documentation for OverflowMenuExtensions.cs, RadioButtonGroupExtensions.cs, RichTextBuilder.cs, SectionBuilder.cs, SelectMenuBaseExtensions.cs, and StaticSelectMenuExtensions.cs
- Completed XML documentation for all code files 