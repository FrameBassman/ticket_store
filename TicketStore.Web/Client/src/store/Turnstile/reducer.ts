import { verifyType } from './actions';
import { VerifyState } from './state';

const initialState: VerifyState = { pass: false };

export const reducer = (state: any, action: any) => {
    state = state || initialState;

    if (action.type == verifyType) {
        const message = action.payload.data;
        let result: boolean;
        if (message == 'OK') {
            result = true;
        } else {
            result = false;
        }

        return { ...state, pass: result }
    }

    return state;
}