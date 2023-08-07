import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { SeleniumLogDto } from '../../../types/CrawlerTypes';

interface CrawlerLiveState {
  logs: SeleniumLogDto[];
}

const initialState: CrawlerLiveState = {
  logs: [],
};

const crawlerLiveSlice = createSlice({
  name: 'crawlerLive',
  initialState,
  reducers: {
    addLog: (state, action: PayloadAction<SeleniumLogDto>) => {
      state.logs.push(action.payload);
    },
  },
});

export const { addLog } = crawlerLiveSlice.actions;
export default crawlerLiveSlice.reducer;