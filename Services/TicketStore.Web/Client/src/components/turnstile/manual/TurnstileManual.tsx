import React, { useState } from 'react';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Container from '@material-ui/core/Container';
import Box from '@material-ui/core/Box';

import { actionCreators } from '../../../store/Turnstile/actions';
import './TurnstileManual.css';

const TurnstileManual = ({ verify }) => {
  const [ticketNumber, setTicketNumber] = useState('');

  return (
    <Container className="turnstile__manual" fixed>
      <Box>
        <TextField
          label="Номер Билета"
          onChange={(e) => setTicketNumber(e.target.value)}
          margin="normal"
          variant="outlined"
          inputProps={{
            type: 'number'
          }}
        />
      </Box>
      <Box>
        <Button
          variant="contained"
          color="primary"
          size="large"
          onClick={() => verify(ticketNumber)}
        >
          Проверить Билет
        </Button>
      </Box>
    </Container>
  )
}

export default connect(
  (state: any) => state.turnstile,
  actionCreators
)(TurnstileManual);
