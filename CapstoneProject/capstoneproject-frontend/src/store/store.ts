import { configureStore } from '@reduxjs/toolkit'
import crawlerLiveReducer from './features/crawlerLive/crawlerLiveSlice';


export const store = configureStore({
    reducer: {
        //user:userSlice
        crawlerLive: crawlerLiveReducer,
    },
})

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch