import React, { Component } from 'react';
import * as throttle from 'lodash/throttle';
import { connect } from 'react-redux';

import { actionCreators } from '../../../store/Turnstile/actions';
import { Scanner } from './Scanner';
import { beep } from './Beep';
import { Status } from './Status';
import { DetectedBarcode } from './DetectedBarcode';
import './Camera.css';

export class CameraTurnstile extends Component<any> {
  constructor(props: any) {
    super(props);
    this._onDetected = throttle(
      this._onDetected.bind(this),
      3000,
      { 'trailing': false }
    );
  }

  _onDetected(current: DetectedBarcode) {
    beep();
    const { code } = current.codeResult;
    this.props.verify(code);
  }

  render() {
    const { pass, wait, scannedTicket } = this.props;
    return (
      <div className="camera-container">
        <Status pass={pass} wait={wait} scannedTicket={scannedTicket} />
        <Scanner onDetected={this._onDetected}/>
      </div>
    );
  }
}

export default connect(
  (state: any) => state.turnstile,
  actionCreators
)(CameraTurnstile);
