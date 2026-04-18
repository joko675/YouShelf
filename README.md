# Backend installation
## Download newest version of YouShelf backend from releases, unzip and run YouShelf.exe

# YouShelf API Docs
## Auth

### POST /auth/register
Registers new user with username and password, returns newly created user id
#### Example request
```json
{
  "username":"test",
  "password":"test"
}
```
#### Example response
```json
{
  "success": true,
  "userId": 2,
  "errorMessage": null
}
```
### POST /auth/login
Authenticate user, return jwt token
#### Example request
```json
{
  "username":"test",
  "password":"test"
}
```
#### Example response
```json 
{
  "success": true,
  "token": "testToken",
  "errorMessage": null
}
```

## Book
### POST /book
Add new book. Returns newly created book info. Require authorization header
#### Header 
```http
Authorization: Bearer <jwt token>
```
#### Parameters
| Parameter  | Type  			   | Required|
|------------|---------------------|---------|
| title      | string              | true    |
| author     | string              | false   |
| description| string		   	   | false   |
| releaseDate| string(yyyy-MM-dd)  | false   |
| imageUrl   | string              | false   |
| review     | string              | false   |
| status     | Status(see below)   | true    |

Status enum - allowed text: READING, READ, PLANNING, CANCELLED<br>
ImageUrl - default placeholder image
#### Example request
```json
{
  "title": "Harry Potter",
  "author": "J.K. Rowling",
  "description": "TestDescription",
  "releaseDate": "2001-01-01",
  "imageUrl": "TestUrl",
  "review": "TestReview",
  "status": "READING"
}
```
#### Example response
```json
{
  "bookId": 14,
  "title": "Harry Potter",
  "author": "J.K. Rowling",
  "description": "TestDescription",
  "releaseDate": "2001-01-01",
  "imageUrl": "TestUrl",
  "review": "TestReview",
  "status": "READING",
  "userId": 1
}
```
### GET /book
Get list of books owned by currently logged user. Require authorization header
#### Header 
```http
Authorization: Bearer <jwt token>
```
#### Parameters
| Parameter | Type   | Required | Default | Description              |
|-----------|--------|----------|---------|--------------------------|
| title     | string | false    | null    | Filters books by title   |
| author	| string | false	| null	  | Filters books by author	 |
| limit		| int	 | false	| 10	  | Maximum number of results|
| offset	| int	 | false	| 0		  | How many results to skip |
| status    | Status | false    | null    | Filter books by status   |

Status enum - allowed text: "READING", "READ", "PLANNING", "CANCELLED"
#### Example request
`/book?title=Harry+Potter&author=J.K.+Rowling&limit=10&offset=0&status=READING`
#### Example response
```json
[
  {
    "bookId": 1,
    "title": "Harry Potter",
    "author": "J.K. Rowling",
    "description": "TestDescription",
    "releaseDate": "2001-01-01",
    "imageUrl": "TestUrl",
    "review": "TestReview",
    "status": "READING",
    "userId": 1
  }
]
```
### PUT /book
Updates existing book. User can only update his own books. Returns updated book info. Require authorization header.
#### Header 
```http
Authorization: Bearer <jwt token>
```
#### Example request
`/book?bookid=1`
```json
{
  "title": "Harry Potter",
  "author": "J.K. Rowling",
  "description": "TestDescription",
  "releaseDate": "2024-01-01",
  "imageUrl": "TestUrl",
  "review": "TestReview",
  "status": "READING"
}
```
#### Example response
```json
{
  "bookId": 1,
  "title": "Harry Potter",
  "author": "J.K. Rowling",
  "description": "TestDescription",
  "releaseDate": "2024-01-01",
  "imageUrl": "TestUrl",
  "review": "TestReview",
  "status": "READING",
  "userId": 1
}
```
### DELETE /book
Deletes book with given id. User can only delete his own books. Require authorization header.
#### Example request 
` /book?bookId=1 `