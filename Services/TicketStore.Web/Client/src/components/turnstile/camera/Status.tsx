import React from 'react';
import { connect } from 'react-redux';
import Fab from '@material-ui/core/Fab';
import CheckIcon from '@material-ui/icons/Check';
import CancelIcon from '@material-ui/icons/Cancel';
import ScannerIcon from '@material-ui/icons/Scanner';
import { createMuiTheme, MuiThemeProvider } from '@material-ui/core/styles';
import green from '@material-ui/core/colors/green';
import red from '@material-ui/core/colors/red';

import './Status.css';
import { ScannedTicket } from '../TurnstileState';

const theme = createMuiTheme({
  palette: {
    primary: green,
    secondary: red,
  }
});

const Description = ({ message }: { message: string }) => {
  return (
    <span className="description">{message}</span>
  )
}

const TicketInfo = ({ label }: { label: string, status?: string }) => {
  return (
    <div className="ticket-info">
        <span><b>Событие:</b> {label}</span>
    </div>
  )
}

const Container = ({ children }) => (
  <MuiThemeProvider theme={theme}>
    <div className="status-container">
      {children}
    </div>
  </MuiThemeProvider>
)

type Props = {
  ticketFound?: boolean,
  wait?: boolean,
  scannedTicket?: ScannedTicket,
};

export const Status = ({ ticketFound, wait, scannedTicket }: Props) => {
  if (!wait) {
    return (
      <Container>
        <Fab><ScannerIcon /></Fab>
        <Description message="Готов к проверке!"/>
        <TicketInfo label="" />
      </Container>
    )
  }

  if (scannedTicket) {
    const { used, concertLabel } = scannedTicket;
    if (ticketFound && !used) {
      return (
        <Container>
          <Fab color="primary"><CheckIcon /></Fab>
          <Description message="Билет Действителен" />
          <TicketInfo label={concertLabel}/>
        </Container>
      )
    }
  
    if (ticketFound && used) {
      return (
        <Container>
          <Fab color="secondary"><CancelIcon /></Fab>
          <Description message="Билет Использован"/>
          <TicketInfo label={concertLabel} />
        </Container>
      )
    }
  }  

  return (
    <Container>
      <Fab color="secondary"><CancelIcon /></Fab>
      <Description message="Билет не найден"/>
      <TicketInfo label="" />
    </Container>
  )
}

export default connect(
  (state: any) => state.turnstile,
)(Status);
