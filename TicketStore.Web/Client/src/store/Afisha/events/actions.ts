import axios from 'axios';
import { eventsUrl, merchantsUrl } from '../urls';
import { eventsFetchDataSuccessType, eventsHasErroredType, eventsIsLoadingType } from './types';
import { merchantsFetchData } from '../merchants/actions';
import { merchantsHasErrored, merchantsIsLoading, merchantsFetchDataSuccess } from '../merchants/actions';

export function eventsFetchData(merchantId: number) {
  return (dispatch) => {
    dispatch(eventsIsLoading(true));

    axios.get(eventsUrl, { params: { merchantId }})
      .then((response) => {
        if (response.status !== 200) {
          throw Error(response.statusText);
        }

        dispatch(eventsIsLoading(false));

        return response;
      })
      .then((response) => response.data)
      .then((items) => dispatch(eventsFetchDataSuccess(items)))
      .catch(() => dispatch(eventsHasErrored(true)));
  };
}

export function allEventsFetch() {
  return (dispatch) => {
    dispatch(merchantsIsLoading(true));

    axios.get(merchantsUrl)
      .then((res) => {
        if (res.status !== 200) {
          throw Error(res.statusText);
        }

        dispatch(merchantsIsLoading(false));
        return res;
      })
      .then((res) => res.data)
      .then((merchants: any[]) => {
        dispatch(merchantsFetchDataSuccess(merchants))
        return merchants;
      })
      .then((merchants: any[]) => {
        dispatch(eventsIsLoading(true));
        const events: any[] = [];

        merchants.forEach(merchant => {
          axios.get(eventsUrl, { params: { merchantId: merchant.id }})
            .then((response) => {
              if (response.status !== 200) {
                throw Error(response.statusText);
              }
              return response;
            })
            .then((response) => response.data)
            .then((fetchedEvents) => events.push(fetchedEvents))
            .catch(() => dispatch(eventsHasErrored(true)))
        });

        dispatch(eventsIsLoading(false));
        dispatch(eventsFetchDataSuccess(events));
      })
      .catch((err) => {
        console.log('error: ', err);
        dispatch(merchantsHasErrored(true))
      })
  }
}

function eventsHasErrored(bool: boolean) {
  return {
    type: eventsHasErroredType,
    hasErrored: bool
  };
}

function eventsIsLoading(bool: boolean) {
  return {
    type: eventsIsLoadingType,
    isLoading: bool
  };
}

function eventsFetchDataSuccess(events) {
  return {
    type: eventsFetchDataSuccessType,
    events
  };
}
