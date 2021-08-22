import numpy as np
from collections import defaultdict
data_path = 'ml-1m/ratings.dat'
n_users = 6040
n_movies = 3706

def load_rating_data(data_path, n_users, n_movies):
    """
    Load rating data from file and also return the number of
    ratings for each movie and movie_id index mapping
    @param data_path: path of the rating data file
    @param n_users: number of users
    @param n_movies: number of movies that have ratings
    @return: rating data in the numpy array of [user, movie];
    movie_n_rating, {movie_id: number of ratings};
    movie_id_mapping, {movie_id: column index in
    rating data}
    """
    data = np.zeros([n_users, n_movies], dtype=np.float32)
    movie_id_mapping = {}
    movie_n_rating = defaultdict(int)
    with open(data_path, 'r') as file:
        for line in file.readlines()[1:]:
            user_id, movie_id, rating, _ = line.split("::")
            user_id = int(user_id) - 1
            if movie_id not in movie_id_mapping:
                movie_id_mapping[movie_id] = len(movie_id_mapping)
            rating = int(rating)
            data[user_id, movie_id_mapping[movie_id]] = rating
            if rating > 0:
                movie_n_rating[movie_id] += 1
    return data, movie_n_rating, movie_id_mapping

data, movie_n_rating, movie_id_mapping = load_rating_data(data_path, n_users, n_movies)

