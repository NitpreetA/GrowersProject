from abc import ABC, abstractmethod
from enum import Enum
import json


class ACommand(ABC):
    """Abstract class for actuator command. Can be instantiated directly or inherited.
    Also defines all possible command types via enums.
    """

    class Type(str, Enum):
        """Enum defining types of actuators that can be targets for a command
        """
        FAN = 'fan'
        BUZZER = 'buzzer'
        LEDSTRIP_ON_OFF = 'led'
        SERVO = 'servo'

    # Class properties that must be defined in implementation classes

    def __init__(self, target: Type, raw_message_body: str) -> None:
        """Builds an actuator command from a cloud message

        :param Type target: Type actuator targeted by command.
        :param str raw_message_body : Body of the message as received from cloud gateway
        """
        self.target_type = target
        # Parse the C2D message body as a dictionary. The items and structure inside
        # the body are left as an implementation detail of each specific
        # actuator.
        self.data: dict = json.loads(raw_message_body)

    def __repr__(self) -> str:
        return f'Command setting {self.target_type} to {self.value}'


class IActuator(ABC):

    # Class properties that must be set in constructor of implementation class
    _current_state: str
    type: ACommand.Type

    @abstractmethod
    def __init__(
            self,
            gpio: int,
            type: ACommand.Type,
            initial_state: dict) -> None:
        """Constructor for Actuator class. Must define interface's class properties

        :param ACommand.Type type: Type of command the actuator can respond to.
        :param str initial_state: initializes 'current_state' property of a new actuator.
        If not passed, actuator implementation is responsible for setting a default value.
        """
        pass

    @abstractmethod
    def validate_command(self, command: ACommand) -> bool:
        """Validates that a command can be used with the specific actuator.

        :param ACommand command: the command to be validated.
        :return bool: True if command can be consumed by the actuator.
        """
        pass

    @abstractmethod
    def control_actuator(self, data: dict) -> bool:
        """Sets the actuator to the value passed as argument.

        :param ACommand command: the command to control the actuator.
        :return bool: True if the state of the actuator changed, false otherwise.
        """
        pass
