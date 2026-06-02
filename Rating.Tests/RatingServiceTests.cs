using Moq;
using Rating.Application.Dtos;
using Rating.Application.Interfaces;
using Rating.Application.Services;
using Rating.Domain.Entities;
using System.Timers;

namespace Rating.Tests;

public class RatingServiceTests
{
    [Fact]
    public async Task CreateRating_Should_Add_Rating_When_Data_Is_Valid()
    {
        // Arrange
        var repositoryMock = new Mock<IRatingRepository>();
        var service = new RatingService(repositoryMock.Object);

        var dto = new CreateRatingDto
        {
            StudentId = 1,
            Rating = 5
        };

        // Act
        await service.CreateRating(3, dto, CancellationToken.None);

        // Assert
        repositoryMock.Verify(repo => repo.AddAsync(
            It.Is<RatingEntity>(r =>
                r.CourseId == 3 &&
                r.StudentId == 1 &&
                r.Rating == 5),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateRating_Should_Throw_When_Rating_Is_Lower_Than_One()
    {
        // Arrange
        var repositoryMock = new Mock<IRatingRepository>();
        var service = new RatingService(repositoryMock.Object);

        var dto = new CreateRatingDto
        {
            StudentId = 1,
            Rating = 0
        };

        // Act
        var action = async () =>
            await service.CreateRating(3, dto, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(action);

        repositoryMock.Verify(repo => repo.AddAsync(
            It.IsAny<RatingEntity>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateRating_Should_Throw_When_Rating_Is_Higher_Than_Five()
    {
        // Arrange
        var repositoryMock = new Mock<IRatingRepository>();
        var service = new RatingService(repositoryMock.Object);

        var dto = new CreateRatingDto
        {
            StudentId = 1,
            Rating = 6
        };

        // Act
        var action = async () =>
            await service.CreateRating(3, dto, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(action);

        repositoryMock.Verify(repo => repo.AddAsync(
            It.IsAny<RatingEntity>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetRatingSummary_Should_Return_Correct_Average_And_Total()
    {
        // Arrange
        var repositoryMock = new Mock<IRatingRepository>();

        repositoryMock
            .Setup(repo => repo.GetByCourseIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<RatingEntity>
            {
                new RatingEntity { CourseId = 3, StudentId = 1, Rating = 5 },
                new RatingEntity { CourseId = 3, StudentId = 2, Rating = 4 },
                new RatingEntity { CourseId = 3, StudentId = 3, Rating = 3 }
            });

        var service = new RatingService(repositoryMock.Object);

        // Act
        var result = await service.GetRatingSummary(3, CancellationToken.None);

        // Assert
        Assert.Equal(4.0, result.AverageRating);
        Assert.Equal(3, result.TotalReviews);
        Assert.Equal(5, result.Ratings.Count);
    }

    [Fact]
    public async Task GetRatingSummary_Should_Return_Correct_Percentages()
    {
        // Arrange
        var repositoryMock = new Mock<IRatingRepository>();

        repositoryMock
            .Setup(repo => repo.GetByCourseIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<RatingEntity>
            {
                new RatingEntity { CourseId = 3, StudentId = 1, Rating = 5 },
                new RatingEntity { CourseId = 3, StudentId = 2, Rating = 5 },
                new RatingEntity { CourseId = 3, StudentId = 3, Rating = 4 },
                new RatingEntity { CourseId = 3, StudentId = 4, Rating = 3 }
            });

        var service = new RatingService(repositoryMock.Object);

        // Act
        var result = await service.GetRatingSummary(3, CancellationToken.None);

        // Assert
        Assert.Equal(50, result.Ratings.First(r => r.Stars == 5).Percentage);
        Assert.Equal(25, result.Ratings.First(r => r.Stars == 4).Percentage);
        Assert.Equal(25, result.Ratings.First(r => r.Stars == 3).Percentage);
        Assert.Equal(0, result.Ratings.First(r => r.Stars == 2).Percentage);
        Assert.Equal(0, result.Ratings.First(r => r.Stars == 1).Percentage);
    }

    [Fact]
    public async Task GetRatingSummary_Should_Return_Zero_When_No_Ratings_Exist()
    {
        // Arrange
        var repositoryMock = new Mock<IRatingRepository>();

        repositoryMock
            .Setup(repo => repo.GetByCourseIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<RatingEntity>());

        var service = new RatingService(repositoryMock.Object);

        // Act
        var result = await service.GetRatingSummary(3, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.AverageRating);
        Assert.Equal(0, result.TotalReviews);
        Assert.Equal(5, result.Ratings.Count);
        Assert.All(result.Ratings, r => Assert.Equal(0, r.Percentage));
    }
}